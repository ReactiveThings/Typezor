using System;
using System.Collections.Immutable;
using System.Diagnostics;
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
        public Func<SourceProductionContext, ILogger, ITemplateOutput> TemplateOutputFactory { get; set; } = (context, logger) => new SourceProductionContextOutput(context, logger);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            Debugger.Launch();
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

            context.RegisterSourceOutput(razorTemplates.Combine(context.CompilationProvider.Combine(logging)), (productionContext, pair) =>
            {
                try
                {
                    var templateCompilation = pair.Left;
                    foreach (var diagnostic in templateCompilation.Diagnostics)
                    {
                        productionContext.ReportDiagnostic(diagnostic);
                    }

                    if (pair.Left.Diagnostics.Any() || productionContext.CancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    var logger = new SourceProductionContextLogger(productionContext, pair.Right.Right);
                    using (logger.Performance("RenderAsync"))
                    {
                        var output = TemplateOutputFactory(productionContext, logger);
                        var namespaceMetadata = CreateNamespaceMetadata(pair.Right.Left);
                        templateCompilation.RenderAsync(namespaceMetadata, output, productionContext.CancellationToken).Wait();
                    }
                }
                catch (Exception e)
                {
                    productionContext.ReportDiagnostic(e.ToDiagnostic());
                }
            });
        }

        private static FileImpl CreateNamespaceMetadata(Compilation compilation)
        {
            return new FileImpl(new RoslynGlobalNamespaceMetadata(compilation.GlobalNamespace, new FindAllTypesVisitor()));
        }

        private static Func<AnalyzerConfigOptionsProvider, CancellationToken, bool> EmitLogging() =>
            (p, _) =>
                p.GlobalOptions.TryGetValue("typezor_info_as_warning", out var emitLoggingSwitch) &&
                emitLoggingSwitch.Equals("true", StringComparison.OrdinalIgnoreCase);

        private static Func<((string Path, string Text) Left, (ImmutableArray<Assembly> Left, ImmutableArray<string> Right) Right), CancellationToken, TemplateDescriptor> Compile(IAssemblyLoadContext assemblyLoadContext) =>
            (file, token) => TemplateDescriptor.Compile(
                file.Left.Text,
                new ReferenceProvider(assemblyLoadContext).GetStandardAssemblies().Concat(file.Right.Left),
                assemblyLoadContext,
                file.Left.Path,
                file.Right.Right,
                token);

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, (string Path, string Text)> GetTextWithPath() =>
            (p, token) => (p.Left.Path, Text: p.Left.GetText(token)?.ToString());

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, Assembly> LoadAssembly(IAssemblyLoadContext assemblyLoadContext) =>
            (p, _) => assemblyLoadContext.LoadFromAssemblyPath(p.Left.Path);

        private static Func<string, bool> IsNotNullOrWhiteSpace() => content => !string.IsNullOrWhiteSpace(content);

        private static Func<(string Path, string Text), bool> IsTemplateNotEmpty() => content => !string.IsNullOrWhiteSpace(content.Text);

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, string> GetText()
            => (p, token) => p.Left.GetText(token)?.ToString();
    }
}
