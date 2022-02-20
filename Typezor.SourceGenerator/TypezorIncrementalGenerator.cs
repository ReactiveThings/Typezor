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

namespace Typezor.SourceGenerator
{
    [Generator]
    public class TypezorIncrementalGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var assemblyLoadContext = AssemblyLoadContextFactory.Create();

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

        private static Func<(string Path, string Text), bool> IsTemplateNotEmpty()
        {
            return content =>
            {
                Log1.Warn($"IsTemplateNotEmpty {content.Path}");
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

        private static Func<string, bool> IsNotNullOrWhiteSpace()
        {
            return content =>
            {
                Log1.Warn($"IsNotNullOrWhiteSpace { new string(content.Take(50).ToArray()) }");
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

    }
}
