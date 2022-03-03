using System;
using System.IO;
using System.Text;
using System.Threading;
using Typezor.SourceGenerator.Logger;

namespace Typezor.SourceGenerator.TemplateOutput
{
    public abstract class TemplateOutput : ITemplateOutput
    {
        protected readonly ILogger Logger;
        protected readonly CancellationToken CancellationToken;
        protected readonly StringBuilder Builder = new StringBuilder();

        protected TemplateOutput(ILogger logger, CancellationToken cancellationToken)
        {
            Logger = logger;
            CancellationToken = cancellationToken;
        }
        public void Write(string text)
        {
            Builder.Append(text);
        }

        public string SaveAs(string filePath)
        {
            CancellationToken.ThrowIfCancellationRequested();

            var f = new FileInfo(filePath);

            var content = Builder.ToString().Trim();

            long count = Encoding.UTF8.GetByteCount(content) + Encoding.UTF8.GetPreamble().Length;

            if (!f.Exists || f.Length != count)
            {
                if (!f.Exists && f.DirectoryName != null)
                {
                    Directory.CreateDirectory(f.DirectoryName);
                }
                using var _ = Logger.Performance($"SaveAs {filePath}");
                File.WriteAllText(filePath, content, Encoding.UTF8);
            }

            Builder.Clear();
            return String.Empty;
        }

        public abstract string AddSource(string hintName);
    }
}