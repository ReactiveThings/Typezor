using System;
using System.Collections.Generic;
using System.Text;

namespace Typezor.Tests.SourceGenerator.Mocks;

public class TemplateOutputMock : ITemplateOutput
{
    public StringBuilder Output { get; set; } = new();
    public Dictionary<string, string> Sources { get; set; } = new();
    public Dictionary<string, string> Files { get; set; } = new();
    public void Write(string text)
    {
        Output.Append(text);
    }

    public string SaveAs(string filePath)
    {
        Files.Add(filePath, Output.ToString());
        Output.Clear();
        return String.Empty;
    }

    public string AddSource(string hintName)
    {
        Sources.Add(hintName, Output.ToString());
        Output.Clear();
        return String.Empty;
    }
}