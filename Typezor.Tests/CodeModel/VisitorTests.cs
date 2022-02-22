using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.CodeModel
{

    [Trait("CodeModel", "Classes"), Collection(nameof(RoslynFixture))]
    public class RoslynVisitorTests : VisitorTests
    {
        public RoslynVisitorTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class VisitorTests : TestBase
    {
        private readonly File fileInfo;

        protected VisitorTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"CodeModel\Support\ClassInfo.cs", 
                @"CodeModel\Support\EnumInfo.cs", 
                @"CodeModel\Support\IInterfaceInfo.cs",
                @"CodeModel\Support\DelegateInfo.cs",
                @"CodeModel\Support\AttributeInfo.cs"
                );
        }

        [Fact]
        public void ClassesFromAllReferencedAssembliesAreReturnedByDefault()
        {
            var classes = fileInfo.Classes;

            classes.Count(p => p.Namespace != null && !p.Namespace.StartsWith("Typezor")).ShouldBeGreaterThan(0);
        }

        [Fact]
        public void EnumsFromAllReferencedAssembliesAreReturnedByDefault()
        {
            var enums = fileInfo.Enums;

            enums.Count(p => p.Namespace != null && !p.Namespace.StartsWith("Typezor")).ShouldBeGreaterThan(0);
        }

        [Fact]
        public void InterfacesFromAllReferencedAssembliesAreReturnedByDefault()
        {
            var interfaces = fileInfo.Interfaces;

            interfaces.Count(p => p.Namespace != null && !p.Namespace.StartsWith("Typezor")).ShouldBeGreaterThan(0);
        }


        [Fact]
        public void GetTypesFromNamespaceReturnAllClassesFromNamespaceThatStartsWithFilter()
        {
            var classInfo = fileInfo.GetTypesFromNamespace("Typezor.Tests.CodeModel.Support.Class").Classes.ToList();

            classInfo.All(p => p.Namespace.StartsWith("Typezor.Tests.CodeModel.Support.Class")).ShouldBeTrue();

            var expectedClasses = new[]
            {
                "ClassInfo",
                "BaseClassInfo",
                "GenericClassInfo",
                "InheritGenericClassInfo",
                "NestedClassInfo",
                "ClassInNestedNamespaceInfo",
                "ClassInTwoNestedNamespaceInfo"
            }.OrderBy(p => p).ToArray();

            classInfo.Select(p => p.Name).OrderBy(p => p).ToArray().ShouldEqual(expectedClasses);
        }

        [Fact]
        public void GetTypesFromNamespaceReturnAllInterfacesFromNamespaceThatStartsWithFilter()
        {
            var classInfo = fileInfo.GetTypesFromNamespace("Typezor.Tests.CodeModel.Support.Interfaces").Interfaces.ToList();

            classInfo.All(p => p.Namespace.StartsWith("Typezor.Tests.CodeModel.Support.Interfaces")).ShouldBeTrue();

            var expectedClasses = new[]
            {
                "IBaseInterfaceInfo", "IGenericInterface", "IInheritGenericInterfaceInfo", "IInterfaceInfo", "INestedInterfaceInfo",
                "InterfaceInNestedNamespaceInfo",
                "InterfaceInTwoNestedNamespaceInfo"
            }.OrderBy(p => p).ToArray();

            classInfo.Select(p => p.Name).OrderBy(p => p).ToArray().ShouldEqual(expectedClasses);
        }

        [Fact]
        public void GetTypesFromNamespaceReturnAllEnumsFromNamespaceThatStartsWithFilter()
        {
            var classInfo = fileInfo.GetTypesFromNamespace("Typezor.Tests.CodeModel.Support.Enums").Enums.ToList();

            classInfo.All(p => p.Namespace.StartsWith("Typezor.Tests.CodeModel.Support.Enums")).ShouldBeTrue();

            var expectedClasses = new[]
            {
                "EnumInfo", "FlagsEnumInfo", "HexEnumInfo", "NestedEnumInfo",
                "EnumInNestedNamespaceInfo", "EnumInTwoNestedNamespaceInfo"
            }.OrderBy(p => p).ToArray();

            classInfo.Select(p => p.Name).OrderBy(p => p).ToArray().ShouldEqual(expectedClasses);
        }

        [Fact]
        public void GetTypesFromNamespaceReturnAllClassesFromNestedNamespaces()
        {
            var classInfo = fileInfo.GetTypesFromNamespace("Typezor.Tests.CodeModel.Support.Class.NestedNamespace").Classes.ToList();

            classInfo.All(p => p.Namespace.StartsWith("Typezor.Tests.CodeModel.Support.Class.NestedNamespace")).ShouldBeTrue();

            var expectedClasses = new[]
            {
                "ClassInNestedNamespaceInfo",
                "ClassInTwoNestedNamespaceInfo"
            }.OrderBy(p => p).ToArray();

            classInfo.Select(p => p.Name).OrderBy(p => p).ToArray().ShouldEqual(expectedClasses);
        }

        [Fact]
        public void GetTypesFromNamespaceReturnAllEnumsFromNestedNamespaces()
        {
            var classInfo = fileInfo.GetTypesFromNamespace("Typezor.Tests.CodeModel.Support.Enums.NestedNamespace").Enums.ToList();

            classInfo.All(p => p.Namespace.StartsWith("Typezor.Tests.CodeModel.Support.Enums.NestedNamespace")).ShouldBeTrue();

            var expectedClasses = new[]
            {
                "EnumInNestedNamespaceInfo", "EnumInTwoNestedNamespaceInfo"
            }.OrderBy(p => p).ToArray();

            classInfo.Select(p => p.Name).OrderBy(p => p).ToArray().ShouldEqual(expectedClasses);
        }

        [Fact]
        public void GetTypesFromNamespaceReturnAllInterfacesFromNestedNamespaces()
        {
            var classInfo = fileInfo.GetTypesFromNamespace("Typezor.Tests.CodeModel.Support.Interfaces.NestedNamespace").Interfaces.ToList();

            classInfo.All(p => p.Namespace.StartsWith("Typezor.Tests.CodeModel.Support.Interfaces.NestedNamespace")).ShouldBeTrue();

            var expectedClasses = new[]
            {
                "InterfaceInNestedNamespaceInfo",
                "InterfaceInTwoNestedNamespaceInfo"
            }.OrderBy(p => p).ToArray();

            classInfo.Select(p => p.Name).OrderBy(p => p).ToArray().ShouldEqual(expectedClasses);
        }
    }
}