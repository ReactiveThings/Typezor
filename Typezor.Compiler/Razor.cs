using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace Typezor;

/// <summary>
/// Exception thrown when an attempt to compile a Razor template fails.
/// </summary>
public class TranspileRazorException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    public readonly IReadOnlyList<RazorDiagnostic> Diagnostics;

    /// <summary>
    /// Initializes an instance of <see cref="CompilationException"/>.
    /// </summary>
    public TranspileRazorException(string message, IReadOnlyList<RazorDiagnostic> diagnostics) : base(message)
    {
        Diagnostics = diagnostics;
    }
}

/// <summary>
/// Methods to transpile and compile Razor templates.
/// </summary>
public static class Razor
{
    private static string? TryGetNamespace(string razorCode) =>
        Regex.Matches(razorCode, @"^\@namespace\s+(.+)$", RegexOptions.Multiline, TimeSpan.FromSeconds(1))
            .Cast<Match>()
            .LastOrDefault()?
            .Groups[1]
            .Value
            .Trim();

    /// <summary>
    /// Transpiles a Razor template into C# code.
    /// </summary>
    public static string Transpile(
        string source,
        string path,
        string? accessModifier = null,
        Action<RazorProjectEngineBuilder>? configure = null)
    {
        // For some reason Razor engine ignores @namespace directive if
        // the file system is not configured properly.
        // So to work around it, we "parse" it ourselves.
        var actualNamespace =
            TryGetNamespace(source) ??
            "Typezor.GeneratedTemplates";

        var engine = RazorProjectEngine.Create(
            RazorConfiguration.Default,
            EmptyRazorProjectFileSystem.Instance,
            options =>
            {
                SectionDirective.Register(options);
                options.SetNamespace(actualNamespace);
                options.SetBaseType("Typezor.TemplateBase<dynamic>");

                options.ConfigureClass((_, node) =>
                {
                    node.Modifiers.Clear();

                    // Null access modifier resolved to internal by default in C#
                    if (!string.IsNullOrWhiteSpace(accessModifier))
                        node.Modifiers.Add(accessModifier);

                    // Partial to allow extension
                    node.Modifiers.Add("partial");
                });

                configure?.Invoke(options);
            }
        );

        var sourceDocument = RazorSourceDocument.Create(
            source,
            path
        );

        var codeDocument = engine.Process(
            sourceDocument,
            null,
            Array.Empty<RazorSourceDocument>(),
            Array.Empty<TagHelperDescriptor>()
        );

        var document = codeDocument.GetCSharpDocument();
        if (document.Diagnostics.Any(p => p.Severity == RazorDiagnosticSeverity.Error))
        {
            throw new TranspileRazorException($"Cannot transpile template {path}", document.Diagnostics);
        }
        return document.GeneratedCode;
    }

}

[ExcludeFromCodeCoverage]
internal partial class EmptyRazorProjectFileSystem : RazorProjectFileSystem
{
    public static EmptyRazorProjectFileSystem Instance { get; } = new();

    public override IEnumerable<RazorProjectItem> EnumerateItems(string basePath) =>
        Enumerable.Empty<RazorProjectItem>();

    [Obsolete("Use GetItem(string path, string fileKind) instead.")]
    public override RazorProjectItem GetItem(string path) =>
        GetItem(path, null);

    public override RazorProjectItem GetItem(string path, string? fileKind) =>
        new NotFoundProjectItem(string.Empty, path, fileKind);
}

internal partial class EmptyRazorProjectFileSystem
{
    [ExcludeFromCodeCoverage]
    private class NotFoundProjectItem : RazorProjectItem
    {
        public override string BasePath { get; }

        public override string FilePath { get; }

        public override string FileKind { get; }

        public override bool Exists => false;

        public override string PhysicalPath => throw new NotSupportedException();

        public NotFoundProjectItem(string basePath, string path, string? fileKind)
        {
            BasePath = basePath;
            FilePath = path;
            FileKind = fileKind ?? FileKinds.GetFileKindFromFilePath(path);
        }

        public override Stream Read() => throw new NotSupportedException();
    }
}