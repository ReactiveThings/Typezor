using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Typezor.SourceGenerator;
using Typezor.Tests.SourceGenerator.Mocks;

namespace Typezor.Tests.SourceGenerator;

public class GeneratorBaseTests
{
    protected static GeneratorDriverRunResult RunGenerator(TypezorSourceGenerator generator,
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