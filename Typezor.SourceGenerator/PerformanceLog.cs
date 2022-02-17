using System;
using System.Diagnostics;

namespace Typezor.SourceGenerator
{
    public class PerformanceLog : IDisposable
    {
        private readonly string name;
        private readonly Stopwatch stopwatch = new Stopwatch();

        public PerformanceLog(string name)
        {
            this.name = name;
            stopwatch.Start();
        }
        public void Dispose()
        {
            stopwatch.Stop();
            Log1.Warn($"{name} {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}