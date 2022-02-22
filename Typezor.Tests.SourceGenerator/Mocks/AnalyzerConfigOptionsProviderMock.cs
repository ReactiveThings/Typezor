using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Typezor.Tests.SourceGenerator.Mocks;

public class AnalyzerConfigOptionsProviderMock : AnalyzerConfigOptionsProvider
{
    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
    {
        return new AnalyzerConfigOptionsMock(new Dictionary<string, string>());
    }

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
    {
        return new AnalyzerConfigOptionsMock(new Dictionary<string, string> { { "build_metadata.AdditionalFiles.Typezor", "true" } });
    }

    public override AnalyzerConfigOptions GlobalOptions { get; } = new AnalyzerConfigOptionsMock(new Dictionary<string, string> { { "typezor_info_as_warning", "true" } });
}