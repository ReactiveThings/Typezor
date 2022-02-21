using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Typezor.SourceGenerator.Logger;

namespace Typezor.SourceGenerator.TemplateOutput
{
    public class SourceGeneratorOutput : TemplateOutput
    {
        private readonly GeneratorExecutionContext _context;

        public SourceGeneratorOutput(GeneratorExecutionContext context, ILogger logger) : base(logger, context.CancellationToken)
        {
            _context = context;
        }

        public override string AddSource(string hintName)
        {
            using var _ = Logger.Performance($"AddSource {hintName}");
            CancellationToken.ThrowIfCancellationRequested();
            _context.AddSource(hintName, SourceText.From(Builder.ToString(),Encoding.UTF8));
            Builder.Clear();
            return String.Empty;
        }
    }
}