﻿using System;
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
                .Combine(razorReferences.Collect().Combine(razorClasses.Collect()))
                .Select(Compile(assemblyLoadContext))
                .Where(p => p != null);

            context.RegisterSourceOutput(razorTemplates.Combine(context.CompilationProvider), (c, pair) =>
            {
                try
                {
                    if (c.CancellationToken.IsCancellationRequested) return;
                    using (new PerformanceLog("RenderAsync"))
                    {
                        var namespaceMetadata = new FileImpl(new RoslynGlobalNamespaceMetadata(pair.Right.GlobalNamespace, new FindAllTypesVisitor()));
                        var output = new SourceProductionContextOutput(c);
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
