using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Typezor.CodeModel.Implementation;
using Typezor.Metadata.Roslyn;
using Typezor.SourceGenerator.AssemblyLoading;

namespace Typezor.SourceGenerator
{
    public class TypezorSourceGenerator : ISourceGenerator
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
                        new FileImpl(new RoslynGlobalNamespaceMetadata(context.Compilation.GlobalNamespace, new FindAllTypesVisitor()));

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
                                if (compiled.Diagnostics.Any())
                                {
                                    foreach (var diagnostic in compiled.Diagnostics)
                                    {
                                        context.ReportDiagnostic(diagnostic);
                                    }
                                }
                                else
                                {
                                    compiled.RenderAsync(namespaceMetadata, output, context.CancellationToken).Wait();
                                }
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    context.ReportDiagnostic(e.ToDiagnostic());
                }
            }
        }

        private static IEnumerable<(string path, string content)> GetRazorTemplates(GeneratorExecutionContext context)
        {
            foreach (var file in context.AdditionalFiles)
            {
                var isRazorTemplate = context.AnalyzerConfigOptions.IsTemplate(file);

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
                var isRazorReference = context.AnalyzerConfigOptions.IsClass(file);

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
}