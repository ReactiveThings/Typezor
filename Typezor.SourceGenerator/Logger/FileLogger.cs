using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public class FileLogger : Logger
    {
        protected override void ReportDiagnostic(DiagnosticSeverity severity, string message)
        {
            try
            {
                System.IO.File.AppendAllLines(@$"c:\temp\test.log", new List<string>()
                {
                    $"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt} {message}"
                });
            }
            catch
            {
                //file maybe in use
            }
        }
    }
}