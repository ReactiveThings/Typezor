using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public class SourceProductionContextLogger : Logger
    {
        private readonly SourceProductionContext _context;

        public SourceProductionContextLogger(SourceProductionContext context)
        {
            _context = context;
        }
        protected override void ReportDiagnostic(DiagnosticSeverity severity, string message)
        {
            var diagnostic =
                Diagnostic.Create(
                    new DiagnosticDescriptor("TZ001", message,
                        message.Replace(Environment.NewLine, ""), 
                        "TypeRazor",
                        severity,
                        true), 
                    Location.None);

            _context.ReportDiagnostic(diagnostic);
        }
    }
}