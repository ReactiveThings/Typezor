using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Typezor.AssemblyLoading;
using Typezor.CodeModel.Implementation;
using Typezor.Metadata.Roslyn;
using Typezor.SourceGenerator.AssemblyLoading;
using Typezor.SourceGenerator.Logger;
using Typezor.SourceGenerator.TemplateOutput;

namespace Typezor.SourceGenerator
{
    [Generator]
    public class TypezorIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var assemblyLoadContext = AssemblyLoadContextFactory.Create();

            var logging = context.AnalyzerConfigOptionsProvider.Select(EmitLogging());

            var razorClasses = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => p.Right.IsClass(p.Left))
                .Select(GetText())
                .Where(IsNotNullOrWhiteSpace());

            var razorReferences = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => p.Right.IsReference(p.Left))
                .Select(LoadAssembly(assemblyLoadContext));

            var razorTemplates = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => p.Right.IsTemplate(p.Left))
                .Select(GetTextWithPath())
                .Where(IsTemplateNotEmpty())
                .Combine(razorReferences.Collect().Combine(razorClasses.Collect()))
                .Select(Compile(assemblyLoadContext))
                .Where(p => p != null);

            context.RegisterSourceOutput(razorTemplates.Combine(context.CompilationProvider.Combine(logging)), (c, pair) =>
            {
                try
                {
                    new FileLogger().Warn(pair.Right.Right.ToString());
                    ILogger logger = new SourceProductionContextLogger(c, pair.Right.Right);
                    if (c.CancellationToken.IsCancellationRequested) return;
                    using (logger.Performance("RenderAsync"))
                    {
                        var namespaceMetadata = new FileImpl(new RoslynGlobalNamespaceMetadata(pair.Right.Left.GlobalNamespace, new FindAllTypesVisitor()));
                        var output = new SourceProductionContextOutput(c, logger);
                        if (pair.Left.Diagnostics.Any())
                        {
                            foreach (var diagnostic in pair.Left.Diagnostics)
                            {
                                c.ReportDiagnostic(diagnostic);
                            }
                        }
                        else
                        {
                            pair.Left.RenderAsync(namespaceMetadata, output, c.CancellationToken).Wait();
                        }
                    }
                }
                catch (Exception e)
                {
                    c.ReportDiagnostic(e.ToDiagnostic());
                }

            });
        }

        private static Func<AnalyzerConfigOptionsProvider, CancellationToken, bool> EmitLogging()
        {
            return (p,_) =>
            {
                bool emitLogging = false;
                if (p.GlobalOptions.TryGetValue("typezor_emit_logging", out var emitLoggingSwitch))
                {
                    emitLogging = emitLoggingSwitch.Equals("true", StringComparison.OrdinalIgnoreCase);
                }

                return emitLogging;
            };
        }

        private static Func<((string Path, string Text) Left, (ImmutableArray<Assembly> Left, ImmutableArray<string> Right) Right), CancellationToken, TemplateDescriptor> Compile(IAssemblyLoadContext assemblyLoadContext)
        {
            return (file, token) => TemplateDescriptor.Compile(
                file.Left.Text,
                new ReferenceProvider(assemblyLoadContext).GetStandardAssemblies().Concat(file.Right.Left),
                assemblyLoadContext,
                file.Left.Path,
                file.Right.Right,
                token);
        }

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, (string Path, string Text)> GetTextWithPath() => (p, token) => (p.Left.Path, Text: p.Left.GetText(token)?.ToString());
        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, Assembly> LoadAssembly(IAssemblyLoadContext assemblyLoadContext) => (p, token) => assemblyLoadContext.LoadFromAssemblyPath(p.Left.Path);
        private static Func<string, bool> IsNotNullOrWhiteSpace() => content => !string.IsNullOrWhiteSpace(content);
        private static Func<(string Path, string Text), bool> IsTemplateNotEmpty() => content => !string.IsNullOrWhiteSpace(content.Text);
        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, string> GetText() => (p, token) => p.Left.GetText(token)?.ToString();
    }
}
