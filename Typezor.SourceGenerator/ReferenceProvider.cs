using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CSharp.RuntimeBinder;
using Typezor;
using Typezor.AssemblyLoading;
using Typezor.CodeModel;

namespace Typezor.SourceGenerator
{
    public class ReferenceProvider
    {
        private readonly IAssemblyLoadContext assemblyLoadContext;

        public ReferenceProvider(IAssemblyLoadContext assemblyLoadContext)
        {
            this.assemblyLoadContext = assemblyLoadContext;
        }

        private IEnumerable<AssemblyName> StandardAssemblies()
        {
            return new[]
            {
                new AssemblyName("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"),
                new AssemblyName("System.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
                new AssemblyName("System.Collections, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"),
                new AssemblyName("Microsoft.CodeAnalysis.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"),
                typeof(int).Assembly.GetName(),
                typeof(Uri).Assembly.GetName(),
                typeof(Enumerable).Assembly.GetName(),
                typeof(RuntimeBinderException).Assembly.GetName(),
                typeof(ITemplate).Assembly.GetName(),
                typeof(IEnumerable<>).Assembly.GetName(),
                typeof(Regex).Assembly.GetName(),
                typeof(Class).Assembly.GetName()
            };
        }

        public IEnumerable<Assembly> GetReferences(GeneratorExecutionContext context)
        {
            foreach (var assembly in GetStandardAssemblies()) yield return assembly;

            foreach (var parentAssembly in GetRazorReferences(context))
            {
                yield return parentAssembly;
            }
        }

        public IEnumerable<Assembly> GetStandardAssemblies()
        {
            foreach (var standardAssembly in StandardAssemblies())
            {
                yield return assemblyLoadContext.LoadFromAssemblyName(standardAssembly);
            }
        }

        private IEnumerable<Assembly> GetRazorReferences(GeneratorExecutionContext context)
        {

            foreach (var file in context.AdditionalFiles)
            {
                var isRazorReference = IsRazorReference(file, context.AnalyzerConfigOptions);

                if (isRazorReference)
                {
                    yield return assemblyLoadContext.LoadFromAssemblyPath(file.Path);
                }
            }
        }

        public static bool IsRazorReference(AdditionalText file, AnalyzerConfigOptionsProvider analyzerConfigOptions)
        {
            return string.Equals(
                analyzerConfigOptions
                    .GetOptions(file)
                    .TryGetAdditionalFileMetadataValue("IsRazorReference"),
                "true",
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}