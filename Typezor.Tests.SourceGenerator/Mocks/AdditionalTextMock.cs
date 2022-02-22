using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Typezor.Tests.SourceGenerator.Mocks;

public class AdditionalTextMock : AdditionalText
{
    public AdditionalTextMock(string text, string path)
    {
        Text = text;
        Path = path;
    }

    public string Text { get; set; }
    public override SourceText? GetText(CancellationToken cancellationToken = new CancellationToken())
    {
        return SourceText.From(Text);
    }

    public override string Path { get; }
}