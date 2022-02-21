using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public class GeneratorExecutionContextLogger : Logger
    {
        private readonly GeneratorExecutionContext _context;

        public GeneratorExecutionContextLogger(GeneratorExecutionContext context)
        {
            _context = context;
        }

        protected override void ReportDiagnostic(DiagnosticSeverity severity, string message)
        {
            var diagnostic =
                Diagnostic.Create(
                    new DiagnosticDescriptor("TZ001", message,
                        message.Replace(Environment.NewLine, ""),
                        "Typezor",
                        severity,
                        true),
                    Location.None);

            _context.ReportDiagnostic(diagnostic);
        }
    }
}