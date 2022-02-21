using System;
using System.Diagnostics;

namespace Typezor.SourceGenerator.Logger
{
    public class PerformanceLog : IDisposable
    {
        private readonly string _name;
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public PerformanceLog(string name, ILogger logger)
        {
            _name = name;
            _logger = logger;
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _logger.Info($"{_name} {_stopwatch.ElapsedMilliseconds}ms");
        }
    }
}