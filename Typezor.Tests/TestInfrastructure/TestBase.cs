using System.IO;
using Typezor.CodeModel.Implementation;
using Xunit;
using File = Typezor.CodeModel.File;


[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Typezor.Tests.TestInfrastructure
{
    public abstract class TestBase
    {
        protected readonly RoslynMetadataProviderStub MetadataProvider;

        protected TestBase(ITestFixture fixture)
        {
            MetadataProvider = fixture.Provider;
        }

        protected File GetFile(params string[] path)
        {
            var metadata = MetadataProvider.GetFile(path);
            return new FileImpl(metadata);
        }
    }
}