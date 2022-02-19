using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator
{
    public class Log1
    {

        static Log1()
        {

        }

        internal static void Warn(string message)
        {
            ReportDiagnostic(DiagnosticSeverity.Warning, message);
        }

        internal static void Error(string message)
        {
            Console.WriteLine(message);
            ReportDiagnostic(DiagnosticSeverity.Error, message);
        }

        private static void ReportDiagnostic(DiagnosticSeverity severity, string message)
        {
            try
            {
                System.IO.File.AppendAllLines(@$"c:\temp\test.log", new List<string>()
                {
                    $"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt} {message}"
                });
            }
            catch (Exception e)
            {
            }

        }
    }
}