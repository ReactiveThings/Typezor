using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Typezor.CodeModel.Implementation;
using Typezor.Metadata.Roslyn;
using Typezor.SourceGenerator.AssemblyLoading;

namespace Typezor.SourceGenerator
{
    [Generator]
    public class TypezorGenerator : IIncrementalGenerator //, ISourceGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            
            var assemblyLoadContext = AssemblyLoadContextFactory.Create();

            var razorClasses = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => IsRazorClass(p.Left,p.Right))
                .Select((p,a) => p.Left.GetText(a)?.ToString())
                .Where(content => !string.IsNullOrWhiteSpace(content));

            var razorReferences = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => ReferenceProvider.IsRazorReference(p.Left, p.Right))
                .Select((p, a) => assemblyLoadContext.LoadFromAssemblyPath(p.Left.Path));

            var razorTemplates = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => IsRazorTemplate(p.Left, p.Right))
                .Select((p, a) => (p.Left.Path, Text: p.Left.GetText(a)?.ToString()))
                .Where(content => !string.IsNullOrWhiteSpace(content.Text))
                .Combine(razorReferences.Collect().Combine(razorClasses.Collect()))
                .Select((file, token) => TemplateDescriptor.Compile(
                    file.Left.Text,
                    new ReferenceProvider(assemblyLoadContext).GetStandardAssemblies().Concat(file.Right.Left),
                    assemblyLoadContext,
                    file.Left.Path,
                    file.Right.Right,
                    token));

            context.RegisterSourceOutput(razorTemplates.Combine(context.CompilationProvider), (c, pair) =>
            {
                Log1.ExecutionContext = c;
                if (c.CancellationToken.IsCancellationRequested) return;
                using (new PerformanceLog("RenderAsync"))
                {
                    var namespaceMetadata = new FileImpl(new RoslynGlobalNamespaceMetadata(pair.Right.GlobalNamespace));
                    var output = new SourceProductionContextOutput(c);
                    pair.Left.RenderAsync(namespaceMetadata, output, c.CancellationToken).Wait();
                }
            });
        }

        public void Execute(GeneratorExecutionContext context)
        {
            using (new PerformanceLog("TypezorGenerator"))
            {
                Log.ExecutionContext = context;
                try
                {
                    using var _ = new PerformanceLog("Execute");
                    var namespaceMetadata =
                        new FileImpl(new RoslynGlobalNamespaceMetadata(context.Compilation.GlobalNamespace));

                    var assemblyLoadContext = AssemblyLoadContextFactory.Create();

                    var razorReferences = new ReferenceProvider(assemblyLoadContext)
                        .GetReferences(context)
                        .ToList();

                    var razorClasses = GetRazorClasses(context).ToList();
                    var templates = GetRazorTemplates(context);

                    var output = new SourceGeneratorOutput(context);
                    foreach (var template in templates)
                    {
                        if (context.CancellationToken.IsCancellationRequested) return;
                        using (new PerformanceLog($"Template {template.path}"))
                        {
                            TemplateDescriptor compiled;
                            using (new PerformanceLog("    Compile"))
                            {
                                compiled = TemplateDescriptor.Compile(template.content, razorReferences,
                                    assemblyLoadContext, template.path, razorClasses);
                            }

                            if (context.CancellationToken.IsCancellationRequested) return;
                            using (new PerformanceLog("    RenderAsync"))
                            {
                                compiled.RenderAsync(namespaceMetadata, output, context.CancellationToken).Wait();
                            }
                        }

                    }
                }
                catch (CompilationException me)
                {
                    foreach (var diagnostic in me.Diagnostics)
                    {
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString() + e.StackTrace);
                    if (e.InnerException != null)
                    {
                        Log.Error(e.InnerException.ToString());
                    }
                }
            }
        }



        private static IEnumerable<(string path, string content)> GetRazorTemplates(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                var isRazorTemplate = IsRazorTemplate(file, context.AnalyzerConfigOptions);

                if (isRazorTemplate)
                {
                    var content = file.GetText(context.CancellationToken)?.ToString();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        yield return (file.Path, content);
                    }
                }
            }
        }

        private static bool IsRazorTemplate(AdditionalText file, AnalyzerConfigOptionsProvider analyzerConfigOptions)
        {
            return string.Equals(
                analyzerConfigOptions
                    .GetOptions(file)
                    .TryGetAdditionalFileMetadataValue("IsRazorTemplate"),
                "true",
                StringComparison.OrdinalIgnoreCase
            );
        }


        private static IEnumerable<string> GetRazorClasses(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                var isRazorReference = IsRazorClass(file, context.AnalyzerConfigOptions);

                if (isRazorReference)
                {
                    yield return file.GetText()?.ToString();
                }
            }
        }

        private static bool IsRazorClass(AdditionalText file, AnalyzerConfigOptionsProvider analyzerConfigOptions)
        {
            return string.Equals(
               
            analyzerConfigOptions.GetOptions(file)
                    .TryGetAdditionalFileMetadataValue("IsRazorClass"),
                "true",
                StringComparison.OrdinalIgnoreCase
            );
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required
        }

    }
    internal static class SourceGeneratorExtensions
    {
        public static string? TryGetValue(this AnalyzerConfigOptions options, string key) =>
            options.TryGetValue(key, out var value) ? value : null;

        public static string? TryGetAdditionalFileMetadataValue(this AnalyzerConfigOptions options, string propertyName) =>
            options.TryGetValue($"build_metadata.AdditionalFiles.{propertyName}");
    }
}
