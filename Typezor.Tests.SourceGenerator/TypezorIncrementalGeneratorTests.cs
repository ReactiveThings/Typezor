using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Typezor.SourceGenerator;
using Typezor.Tests.SourceGenerator.Mocks;
using Xunit;

namespace Typezor.Tests.SourceGenerator;

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