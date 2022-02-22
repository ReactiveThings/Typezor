using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CSharp.RuntimeBinder;
using Typezor.AssemblyLoading;
using Typezor.CodeModel;

namespace Typezor.SourceGenerator
{
    public class ReferenceProvider
    {
        private readonly IAssemblyLoadContext _assemblyLoadContext;

        public ReferenceProvider(IAssemblyLoadContext assemblyLoadContext)
        {
            this._assemblyLoadContext = assemblyLoadContext;
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
                yield return _assemblyLoadContext.LoadFromAssemblyName(standardAssembly);
            }
        }

        private IEnumerable<Assembly> GetRazorReferences(GeneratorExecutionContext context)
        {

            foreach (var file in context.AdditionalFiles)
            {
                var isRazorReference = context.AnalyzerConfigOptions.IsReference(file);

                if (isRazorReference)
                {
                    yield return _assemblyLoadContext.LoadFromAssemblyPath(file.Path);
                }
            }
        }
    }
}