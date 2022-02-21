using System;
using System.Globalization;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Typezor;

public static class DiagnosticExtensions
{
    public static Diagnostic ToDiagnostic(this Exception e)
    {
        if (e.InnerException != null)
        {
            return Diagnostic.Create(new DiagnosticDescriptor("TZ001", e.InnerException.Message, e.InnerException.ToString().Replace(Environment.NewLine, ""), "Typezor", DiagnosticSeverity.Error, true), Location.None);
        }

        return Diagnostic.Create(new DiagnosticDescriptor("TZ001", e.Message, e.ToString().Replace(Environment.NewLine, ""), "Typezor", DiagnosticSeverity.Error, true), Location.None);
    }

    public static Diagnostic ToDiagnostic(this RazorDiagnostic diagnostic)
    {
        var diagnosticDescriptor = new DiagnosticDescriptor(
            diagnostic.Id,
            "Razor template error",
            diagnostic.GetMessage(CultureInfo.CurrentCulture),
            "Typezor",
            diagnostic.Severity == RazorDiagnosticSeverity.Error
                ? DiagnosticSeverity.Error
                : DiagnosticSeverity.Warning, true);

        var location = Location.Create(diagnostic.Span.FilePath,
            TextSpan.FromBounds(diagnostic.Span.LineIndex + 1, diagnostic.Span.LineIndex + 1),
            new LinePositionSpan(new LinePosition(diagnostic.Span.LineIndex + 1, diagnostic.Span.CharacterIndex),
                new LinePosition(diagnostic.Span.LineIndex + 1,
                    diagnostic.Span.CharacterIndex + diagnostic.Span.Length)));

        return Diagnostic.Create(diagnosticDescriptor, location);
    }

}