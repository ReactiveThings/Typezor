using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Typezor.SourceGenerator;
using Xunit;

namespace Typezor.Tests.SourceGenerator;

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

public class GeneratorBaseTests
{

    protected static GeneratorDriverRunResult RunGenerator(TypezorIncrementalGenerator generator,
        string code, params AdditionalText[] additionalTexts)
    {
        var text = ImmutableArray<AdditionalText>.Empty;
        text = text.AddRange(additionalTexts);

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.AddAdditionalTexts(text);
        driver = driver.WithUpdatedAnalyzerConfigOptions(new AnalyzerConfigOptionsProviderMock());
        driver = driver.RunGeneratorsAndUpdateCompilation(CreateCompilation(code), out var outputCompilation, out var diagnostics);
        return driver.GetRunResult();
    }

    protected static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}

public class TypezorIncrementalGeneratorTests : GeneratorBaseTests
{
    [Fact]
    public void WhenTemplateIsProvidedThenCsharpSourceIsAddedToCompilation()
    {
        const string code = @"";
        const string template = @"@namespace Typezor.Tests
@inherits Typezor.TemplateBase<Typezor.CodeModel.File>

namespace Typezor.Tests
{
    public class GeneratedClass1 {}
}@Output.AddSource(""GeneratedClass1"")";

        const string expected = @"
namespace Typezor.Tests
{
    public class GeneratedClass1 {}
}";

        var generator = new TypezorIncrementalGenerator();

        var runResult = RunGenerator(generator, 
            code, 
            new AdditionalTextMock(template, "template.razor"));

        var results = runResult.Results.Single();

        var sourceOutput = results.GeneratedSources.Single();
        Assert.Equal("GeneratedClass1.cs", sourceOutput.HintName);
        Assert.Equal(expected, sourceOutput.SourceText.ToString());
        Assert.True(!results.Diagnostics.Any(p => p.Severity == DiagnosticSeverity.Error));
        Assert.True(results.Exception is null);
    }

    [Fact]
    public void WhenInfoAsWarningOptionIsTrueThenDoEmitWarningInsteadOfInfoDiagnostics()
    {
        const string code = @"";
        const string template = @"@namespace Typezor.Tests
@inherits Typezor.TemplateBase<Typezor.CodeModel.File>
";

        var generator = new TypezorIncrementalGenerator();

        var runResult = RunGenerator(generator,
            code,
            new AdditionalTextMock(template, "template.razor"));

        Assert.True(!runResult.Diagnostics.Any(p => p.Severity == DiagnosticSeverity.Info));
        Assert.True(runResult.Diagnostics.Any(p => p.Severity == DiagnosticSeverity.Warning));
    }

    [Fact]
    public void WhenTemplateIsProvidedThenFileIsCreated()
    {
        const string code = @"";
        const string template = @"@namespace Typezor.Tests
@inherits Typezor.TemplateBase<Typezor.CodeModel.File>

namespace Typezor.Tests
{
    public class GeneratedClass1{}
}@Output.SaveAs(""GeneratedClass1"")";

        const string expected = @"
namespace Typezor.Tests
{
    public class GeneratedClass1{}
}";

        var generator = new TypezorIncrementalGenerator();
        var output = new TemplateOutputMock();
        generator.TemplateOutputFactory = (_, _) => output;

        var runResult = RunGenerator(generator,
            code,
            new AdditionalTextMock(template, "template.razor"));

        Assert.True(!runResult.Diagnostics.Any(p => p.Severity == DiagnosticSeverity.Error));
        Assert.True(runResult.GeneratedTrees.Length == 0);
        var sourceOutput = output.Files.Single();
        Assert.Equal("GeneratedClass1", sourceOutput.Key);
        Assert.Equal(expected, sourceOutput.Value);
    }

    [Fact]
    public void WhenAdditionalCSharpCodeIsProvidedThenWeCanUseItInTemplate()
    {
        const string code = @"";
        const string additionalCode = @"namespace Typezor.Tests
{
    public class AdditionalClass{}
}";
        const string template = @"@namespace Typezor.Tests
@inherits Typezor.TemplateBase<Typezor.CodeModel.File>
@{
    var a = new AdditionalClass();
}
namespace Typezor.Tests
{
    public class GeneratedClass1 {}
}@Output.SaveAs(""GeneratedClass1"")";

        const string expected = @"namespace Typezor.Tests
{
    public class GeneratedClass1 {}
}";

        var generator = new TypezorIncrementalGenerator();
        var output = new TemplateOutputMock();
        generator.TemplateOutputFactory = (_, _) => output;

        var runResult = RunGenerator(generator,
            code,
            new AdditionalTextMock(template, "template.razor"),
            new AdditionalTextMock(additionalCode, "additionalCode.cs"));

        Assert.True(!runResult.Diagnostics.Any(p => p.Severity == DiagnosticSeverity.Error));
        Assert.True(runResult.GeneratedTrees.Length == 0);
        var sourceOutput = output.Files.Single();
        Assert.Equal("GeneratedClass1", sourceOutput.Key);
        Assert.Equal(expected, sourceOutput.Value);
    }

    [Fact]
    public void WhenAdditionalDllCodeIsProvidedThenWeCanUseItInTemplate()
    {
        const string code = @"";
         string additionalCode = File.ReadAllText("Typezor.Tests.SourceGenerator.ReferencedProject.dll");
        const string template = @"@namespace Typezor.Tests
@inherits Typezor.TemplateBase<Typezor.CodeModel.File>
@{
    var a = new Typezor.Tests.SourceGenerator.ReferencedProject.Class3();
}
namespace Typezor.Tests
{
    public class GeneratedClass1 {}
}@Output.SaveAs(""GeneratedClass1"")";

        const string expected = @"namespace Typezor.Tests
{
    public class GeneratedClass1 {}
}";

        var generator = new TypezorIncrementalGenerator();
        var output = new TemplateOutputMock();
        generator.TemplateOutputFactory = (_, _) => output;

        var runResult = RunGenerator(generator,
            code,
            new AdditionalTextMock(template, "template.razor"),
            new AdditionalTextMock(additionalCode, new FileInfo("Typezor.Tests.SourceGenerator.ReferencedProject.dll").FullName));

        Assert.True(!runResult.Diagnostics.Any(p => p.Severity == DiagnosticSeverity.Error));
        Assert.True(runResult.GeneratedTrees.Length == 0);
        Assert.True(runResult.Results.Single().Exception is null);
        var sourceOutput = output.Files.Single();
        Assert.Equal("GeneratedClass1", sourceOutput.Key);
        Assert.Equal(expected, sourceOutput.Value);
    }
}