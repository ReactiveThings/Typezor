using System.IO;
using System.Linq;
using Should;
using Typezor.Tests.TestInfrastructure;
using Xunit;
using File = Typezor.CodeModel.File;

namespace Typezor.Tests.CodeModel
{

    [Trait("CodeModel", "Files"), Collection(nameof(RoslynFixture))]
    public class RoslynFileTests : FileTests
    {
        public RoslynFileTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class FileTests : TestBase
    {
        private readonly File fileInfo;

        protected FileTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"CodeModel\Support\FileInfo.cs");
        }


        [Fact]
        public void Expect_to_find_public_classes()
        {
            var classInfo1 = fileInfo.Classes.FirstOrDefault(p => p.Name == "PublicClassNoNamespace");
            classInfo1.ShouldNotBeNull();

            var classInfo2 = fileInfo.Classes.FirstOrDefault(p => p.Name == "PublicClass");
            classInfo2.ShouldNotBeNull();
        }

        [Fact]
        public void Expect_to_find_public_delegates()
        {

            var delegateInfo1 = fileInfo.Delegates.FirstOrDefault(p => p.Name == "PublicDelegateNoNamespace");
            delegateInfo1.ShouldNotBeNull();

            var delegateInfo2 = fileInfo.Delegates.FirstOrDefault(p => p.Name == "PublicDelegate");
            delegateInfo2.ShouldNotBeNull();
        }

        [Fact]
        public void Expect_to_find_public_enums()
        {

            var enumInfo1 = fileInfo.Enums.FirstOrDefault(p => p.Name == "PublicEnumNoNamespace");
            enumInfo1.ShouldNotBeNull();

            var enumInfo2 = fileInfo.Enums.FirstOrDefault(p => p.Name == "PublicEnum");
            enumInfo2.ShouldNotBeNull();
        }

        [Fact]
        public void Expect_to_find_public_interfaces()
        {

            var interfaceInfo1 = fileInfo.Interfaces.FirstOrDefault(p => p.Name == "PublicInterfaceNoNamespace");
            interfaceInfo1.ShouldNotBeNull();

            var interfaceInfo2 = fileInfo.Interfaces.FirstOrDefault(p => p.Name == "PublicInterface");
            interfaceInfo2.ShouldNotBeNull();
        }
    }
}