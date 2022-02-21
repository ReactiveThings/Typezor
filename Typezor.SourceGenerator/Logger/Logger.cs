using System;
using Microsoft.CodeAnalysis;

namespace Typezor.SourceGenerator.Logger
{
    public abstract class Logger : ILogger
    {
        protected abstract void ReportDiagnostic(DiagnosticSeverity severity, string message);

        public virtual void Warn(string message)
        {
            ReportDiagnostic(DiagnosticSeverity.Warning, message);
        }

        public virtual void Error(string message)
        {
            ReportDiagnostic(DiagnosticSeverity.Error, message);
        }

        public virtual void Info(string message)
        {
            ReportDiagnostic(DiagnosticSeverity.Info, message);
        }

        public IDisposable Performance(string measureName)
        {
            return new PerformanceLog(measureName, this);
        }
    }
}