using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Typewriter.CLI
{
    public class Options
    {
        [Option('s', "solution", Required = true, HelpText = "Path to solution")]
        public string SolutionPath { get; set; }

        [Option('t', "template", Required = true, HelpText = "Paths to templates")]
        public IEnumerable<string> TemplatePaths { get; set; }

        [Option('d', "design-time", HelpText = "Indicates that a design-time build should be performed")]
        public bool DesignTime { get; set; }

        [Option('c', "clean", Default = false, HelpText = "Indicates that a clean build target should be run before compilation")]
        public bool CleanBeforeCompile { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(options => Run(options));
            stopwatch.Stop();
            Console.WriteLine($"Generated in {TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).Seconds} seconds");
        }

        private static void Run(Options options)
        {
            var typewriter = new Typewriter();
            typewriter.Generate(options.SolutionPath, options.TemplatePaths, new BuildOptions() {
                CleanBeforeCompile = options.CleanBeforeCompile,
                DesignTime = options.DesignTime
            });
        }
    }
}
