
using Xunit;

namespace Typezor.Tests.TestInfrastructure
{
    public class RoslynFixture : ITestFixture
    {
        public RoslynFixture()
        {
            Provider = new RoslynMetadataProviderStub();

        }

        public RoslynMetadataProviderStub Provider { get; }

        public void Dispose()
        {

        }
    }

    [CollectionDefinition(nameof(RoslynFixture))]
    public class RoslynCollection : ICollectionFixture<RoslynFixture>
    {
    }
}
