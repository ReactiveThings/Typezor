using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Typezor.Metadata.Interfaces;
using Typezor.Metadata.Roslyn;


namespace Typezor.Tests.TestInfrastructure
{
    public class RoslynMetadataProviderStub
    {

        public RoslynMetadataProviderStub()
        {

        }

        public IFileMetadata GetFile(params string[] path)
        {
            var compilation = CSharpCompilation.Create("compilation",
                path.Select(fileName => CSharpSyntaxTree.ParseText(File.ReadAllText(fileName))),
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
            return new RoslynGlobalNamespaceMetadata(compilation.GlobalNamespace, new FindAllTypesVisitor());
        }
    }
}
