using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public class GeneratorExecutionContextLogger : Logger
    {
        private readonly GeneratorExecutionContext _context;
        private readonly bool _treatInfoAsWarning;

        public GeneratorExecutionContextLogger(GeneratorExecutionContext context, bool treatInfoAsWarning)
        {
            _context = context;
            _treatInfoAsWarning = treatInfoAsWarning;
        }

        public override void Info(string message)
        {
            if (_treatInfoAsWarning)
            {
                Warn(message);
            }
            else
            {
                base.Info(message);
            }

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