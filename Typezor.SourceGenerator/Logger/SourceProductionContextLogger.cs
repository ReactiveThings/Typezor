using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public class SourceProductionContextLogger : Logger
    {
        private readonly SourceProductionContext _context;
        private readonly bool _treatInfoAsWarning;

        public SourceProductionContextLogger(SourceProductionContext context, bool treatInfoAsWarning)
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