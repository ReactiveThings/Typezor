using System;

namespace Typezor.Tests.TestInfrastructure
{
    public interface ITestFixture : IDisposable
    {
        RoslynMetadataProviderStub Provider { get; }
    }
}