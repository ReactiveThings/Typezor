using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public abstract class Logger : ILogger
    {
        protected abstract void ReportDiagnostic(DiagnosticSeverity severity, string message);

        public void Warn(string message)
        {
            ReportDiagnostic(DiagnosticSeverity.Warning, message);
        }

        public void Error(string message)
        {
            ReportDiagnostic(DiagnosticSeverity.Error, message);
        }

        public IDisposable Performance(string measureName)
        {
            return new PerformanceLog(measureName, this);
        }
    }
}