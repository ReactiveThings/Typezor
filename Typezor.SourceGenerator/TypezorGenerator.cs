using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Typezor;
using Typezor.CodeModel.Implementation;
using Typezor.Metadata.Roslyn;
using Typezor.SourceGenerator.AssemblyLoading;
using File = Typezor.CodeModel.File;

namespace Typezor.SourceGenerator
{
    [Generator]
    public class TypezorGenerator : ISourceGenerator
    {
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
                                compiled.RenderAsync(namespaceMetadata, template.path, output,
                                    context.CancellationToken).Wait();
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
                var isRazorTemplate = string.Equals(
                    context.AnalyzerConfigOptions
                        .GetOptions(file)
                        .TryGetAdditionalFileMetadataValue("IsRazorTemplate"),
                    "true",
                    StringComparison.OrdinalIgnoreCase
                );

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



        private static IEnumerable<string> GetRazorClasses(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                var isRazorReference = string.Equals(
                    context.AnalyzerConfigOptions
                        .GetOptions(file)
                        .TryGetAdditionalFileMetadataValue("IsRazorClass"),
                    "true",
                    StringComparison.OrdinalIgnoreCase
                );

                if (isRazorReference)
                {
                    yield return file.GetText()?.ToString();
                }
            }
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
