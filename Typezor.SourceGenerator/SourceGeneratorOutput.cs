using System;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Typezor;

namespace Typezor.SourceGenerator
{
    public class SourceProductionContextOutput : ITemplateOutput
    {
        private readonly SourceProductionContext context;
        private readonly StringBuilder builder = new StringBuilder();


        public SourceProductionContextOutput(SourceProductionContext context)
        {
            this.context = context;
        }

        public void Write(string text)
        {
            builder.Append(text);
        }

        public string SaveAs(string filePath)
        {
            using var _ = new PerformanceLog($"SaveAs {filePath}");
            var f = new FileInfo(filePath);

            var content = builder.ToString().Trim();

            long count = Encoding.UTF8.GetByteCount(content) + Encoding.UTF8.GetPreamble().Length;
            if (!f.Exists || f.Length != count)
            {
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }

            builder.Clear();
            return String.Empty;
        }

        public string AddSource(string hintName)
        {
            using var _ = new PerformanceLog($"AddSource {hintName}");
            context.AddSource(hintName, SourceText.From(builder.ToString(), Encoding.UTF8));
            builder.Clear();
            return String.Empty;
        }
    }

    public class SourceGeneratorOutput : ITemplateOutput
    {
        private readonly GeneratorExecutionContext context;
        private readonly StringBuilder builder = new StringBuilder();

        public SourceGeneratorOutput(GeneratorExecutionContext context)
        {
            this.context = context;
        }

        public void Write(string text)
        {
            builder.Append(text);
        }

        public string SaveAs(string filePath)
        {
            using var _ = new PerformanceLog($"SaveAs {filePath}");
            var f = new FileInfo(filePath);

            var content = builder.ToString().Trim();

            long count = Encoding.UTF8.GetByteCount(content) + Encoding.UTF8.GetPreamble().Length;
            if (!f.Exists || f.Length != count)
            {
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }

            builder.Clear();
            return String.Empty;
        }

        public string AddSource(string hintName)
        {
            using var _ = new PerformanceLog($"AddSource {hintName}");
            context.AddSource(hintName,SourceText.From(builder.ToString(),Encoding.UTF8));
            builder.Clear();
            return String.Empty;
        }
    }
}