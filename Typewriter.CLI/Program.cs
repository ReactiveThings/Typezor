using CommandLine;

namespace Typewriter.CLI
{
    public class Options
    {
        [Option('s', "solution", Required = true, HelpText = "Path to solution")]
        public string SolutionPath { get; set; }

        [Option('t', "template", Required = true, HelpText = "Path to template")]
        public string TemplatePath { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed(options => Run(options));
        }

        private static void Run(Options options)
        {
            var typewriter = new Typewriter();
            typewriter.Generate(options.SolutionPath, options.TemplatePath);
        }
    }
}
