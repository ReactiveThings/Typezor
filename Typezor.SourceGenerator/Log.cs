using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator
{
    public class Log
    {
        public static GeneratorExecutionContext ExecutionContext { get; set; }

        internal static void Warn(string message)
        {
            Console.WriteLine(message);
            ReportDiagnostic(DiagnosticSeverity.Warning, message);
        }

        internal static void Error(string message)
        {
            Console.WriteLine(message);
            ReportDiagnostic(DiagnosticSeverity.Error, message);
        }

        private static void ReportDiagnostic(DiagnosticSeverity severity, string message)
        {
            ExecutionContext.ReportDiagnostic(Diagnostic.Create(
                                    new DiagnosticDescriptor(
                                    "TW0001",
                                    "Typezor code generation",
                                    message.Replace(Environment.NewLine, ""),
                                    "yeet",
                                    severity,
                                    true), null));
        }
    }

}
