using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Typezor.AssemblyLoading;

namespace Typezor;

internal static class AssemblyExtensions
{
    public static MetadataReference ToMetadataReference(this Assembly assembly) =>
        MetadataReference.CreateFromFile(assembly.Location);

    public static bool Implements(this Type type, Type interfaceType) =>
        type.GetInterfaces().Any(c => c.FullName.Contains(interfaceType.FullName));

}

/// <summary>
/// Exception thrown when an attempt to compile a Razor template fails.
/// </summary>
public class CompilationException : Exception
{
    public readonly Diagnostic[] Diagnostics;

    /// <summary>
    /// Initializes an instance of <see cref="CompilationException"/>.
    /// </summary>
    public CompilationException(string message, Diagnostic[] diagnostics) : base(message)
    {
        Diagnostics = diagnostics;
    }
}

public class Compiler
{
    private readonly IAssemblyLoadContext assemblyLoadContext;

    public Compiler(IAssemblyLoadContext assemblyLoadContext)
    {
        this.assemblyLoadContext = assemblyLoadContext;
    }

    public Type Compile<TInterface>(
        IEnumerable<string> sources,
        IEnumerable<Assembly> references, CancellationToken cancellationToken = default
    )
    {
        var csharpClassesAst = sources.Select(p => CSharpSyntaxTree.ParseText(p));

        var csharpDocumentCompilation = CSharpCompilation.Create(
            Guid.NewGuid().ToString(),
            csharpClassesAst.ToArray(),
            references.Select(p => p.ToMetadataReference()),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );


        using var assemblyStream = new MemoryStream();
        var csharpDocumentCompilationResult = csharpDocumentCompilation.Emit(assemblyStream,cancellationToken: cancellationToken);

        if (!csharpDocumentCompilationResult.Success)
        {
            var errors = csharpDocumentCompilationResult
                .Diagnostics
                .Where(d => d.Severity >= DiagnosticSeverity.Error)
                .ToArray();

            throw new CompilationException(
                "Failed to compile template.", errors
            );
        }
        assemblyStream.Seek(0, SeekOrigin.Begin);
        var resultAssembly = assemblyLoadContext.LoadFromStream(assemblyStream);
        return LoadTypeByInterface<TInterface>(resultAssembly);
    }


    private Type LoadTypeByInterface<T>(Assembly assembly)
    {
        var type = assembly
            .GetTypes()
            .FirstOrDefault(t => t.Implements(typeof(T)));

        if (type == null)
        {
            var allImplementedInterfaces = assembly
                .GetTypes()
                .SelectMany(p => p
                    .GetInterfaces()
                    .Select(c => c.FullName));
            var delimitedInterfaceNames = String.Join(" | ", allImplementedInterfaces);
            var exceptionMessage =
                $"Could not locate type that implements {typeof(T).FullName} in the generated assembly. " +
                $"Other implemented interfaces : {delimitedInterfaceNames}";
            throw new InvalidOperationException(exceptionMessage);
        }

        return type;
    }

}