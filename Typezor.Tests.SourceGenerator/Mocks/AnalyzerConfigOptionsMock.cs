using System.Collections.Generic;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Typezor.Tests.SourceGenerator.Mocks;

public class AnalyzerConfigOptionsMock : AnalyzerConfigOptions
{
    private readonly Dictionary<string, string> _dictionary;

    public AnalyzerConfigOptionsMock(Dictionary<string, string> dictionary)
    {
        _dictionary = dictionary;
    }
    public override bool TryGetValue(string key, out string? value)
    {
        if ("build_metadata.AdditionalFiles.Typezor" == key)
        {
            value = "true";
            return true;
        }

        return _dictionary.TryGetValue(key, out value);
    }
}