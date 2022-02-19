using System;
using System.Collections.Generic;
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
                .Select(GetText())
                .Where(IsNullOrWhiteSpace());

            var razorReferences = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => ReferenceProvider.IsRazorReference(p.Left, p.Right))
                .Select(LoadAssembly(assemblyLoadContext));

            var razorTemplates = context.AdditionalTextsProvider.Combine(context.AnalyzerConfigOptionsProvider)
                .Where(p => IsRazorTemplate(p.Left, p.Right))
                .Select(GetTextWithPath())
                .Where(IsTemplateEmpty())
                .Combine(CombineRefAndClass(razorReferences, razorClasses))
                .Select(Compile(assemblyLoadContext))
                .Where(p => p != null);

            context.RegisterSourceOutput(razorTemplates.Combine(context.CompilationProvider), (c, pair) =>
            {
                try
                {
                    Log1.Warn($"RegisterSourceOutput {pair.Left.Path}");
                    if (c.CancellationToken.IsCancellationRequested) return;
                    using (new PerformanceLog("RenderAsync"))
                    {
                        var namespaceMetadata = new FileImpl(new RoslynGlobalNamespaceMetadata(pair.Right.GlobalNamespace, new FindAllTypesVisitor()));
                        var output = new SourceProductionContextOutput(c);
                        pair.Left.RenderAsync(namespaceMetadata, output, c.CancellationToken).Wait();
                    }
                }
                catch (Exception e)
                {
                    Log1.Warn(e.ToString());
                }

            });
        }

        private static IncrementalValueProvider<(ImmutableArray<Assembly> Left, ImmutableArray<string> Right)> CombineRefAndClass(IncrementalValuesProvider<Assembly> razorReferences, IncrementalValuesProvider<string> razorClasses)
        {
            Log1.Warn($"CombineRefAndClass");
            return CollectRazorReferences(razorReferences).Combine(CollectRazorClasses(razorClasses));
        }

        private static IncrementalValueProvider<ImmutableArray<Assembly>> CollectRazorReferences(IncrementalValuesProvider<Assembly> razorReferences)
        {
            Log1.Warn($"CollectRazorReferences");
            return razorReferences.Collect();
        }

        private static IncrementalValueProvider<ImmutableArray<string>> CollectRazorClasses(IncrementalValuesProvider<string> razorClasses)
        {
            Log1.Warn($"CollectRazorClasses");
            return razorClasses.Collect();
        }

        private static Func<(string Path, string Text), bool> IsTemplateEmpty()
        {
            return content =>
            {
                Log1.Warn($"IsTemplateEmpty {content.Path}");
                return !string.IsNullOrWhiteSpace(content.Text);
            };
        }

        private static Func<((string Path, string Text) Left, (ImmutableArray<Assembly> Left, ImmutableArray<string> Right) Right), CancellationToken, TemplateDescriptor> Compile(IAssemblyLoadContext assemblyLoadContext)
        {
            return (file, token) =>
            {
                Log1.Warn($"Compile {file.Left.Path}");
                try
                {
                    return TemplateDescriptor.Compile(
                        file.Left.Text,
                        new ReferenceProvider(assemblyLoadContext).GetStandardAssemblies().Concat(file.Right.Left),
                        assemblyLoadContext,
                        file.Left.Path,
                        file.Right.Right,
                        token);
                }
                catch (Exception e)
                {
                    Log1.Warn(e.ToString());
                    return null;
                }

            };
        }

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, (string Path, string Text)> GetTextWithPath()
        {
            return (p, a) =>
            {
                Log1.Warn($"GetTextWithPath {p.Left.Path}");
                return (p.Left.Path, Text: p.Left.GetText(a)?.ToString());
            };
        }

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, Assembly> LoadAssembly(IAssemblyLoadContext assemblyLoadContext)
        {
            return (p, a) =>
            {
                Log1.Warn($"LoadAssembly {p.Left.Path}");
                return assemblyLoadContext.LoadFromAssemblyPath(p.Left.Path);
            };
        }

        private static Func<string, bool> IsNullOrWhiteSpace()
        {
            return content =>
            {
                Log1.Warn($"IsNullOrWhiteSpace { new string(content.Take(50).ToArray()) }");
                return !string.IsNullOrWhiteSpace(content);
            };
        }

        private static Func<(AdditionalText Left, AnalyzerConfigOptionsProvider Right), CancellationToken, string> GetText()
        {
            return (p, a) =>
            {
                Log1.Warn($"GetText {p.Left.Path}");
                return p.Left.GetText(a)?.ToString();
            };
        }

        public void Execute(GeneratorExecutionContext context)
        {
            return;
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
            Log1.Warn($"IsRazorTemplate {file.Path}");
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
            Log1.Warn($"IsRazorClass {file.Path}");
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
