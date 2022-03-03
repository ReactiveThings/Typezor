using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.CodeModel
{

    [Trait("CodeModel", "Records"), Collection(nameof(RoslynFixture))]
    public class RoslynRecordTests : RecordTests
    {
        public RoslynRecordTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class RecordTests : TestBase
    {
        private readonly File fileInfo;

        protected RecordTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"CodeModel\Support\RecordInfo.cs", 
                @"CodeModel\Support\AttributeInfo.cs", 
                @"CodeModel\Support\IInterfaceInfo.cs"
                );
        }

        [Fact]
        public void Expect_to_be_record()
        {
            var classInfo = fileInfo.Classes.First();

            classInfo.IsRecord.ShouldBeTrue();
        }

        [Fact]
        public void Expect_base_class_to_be_record()
        {
            var baseClass = fileInfo.Classes.First().BaseClass;

            baseClass.IsRecord.ShouldBeTrue();
        }

        [Fact]
        public void Expect_name_to_match_class_name()
        {
            var classInfo = fileInfo.Classes.First();

            classInfo.Name.ShouldEqual("RecordInfo");
            classInfo.FullName.ShouldEqual("Typezor.Tests.CodeModel.Support.Record.RecordInfo");
            classInfo.Namespace.ShouldEqual("Typezor.Tests.CodeModel.Support.Record");
            classInfo.Parent.ShouldEqual(fileInfo);
        }

        [Fact]
        public void Expect_to_find_doc_comment()
        {
            var classInfo = fileInfo.Classes.First();
            classInfo.DocComment.Summary.ShouldEqual("summary");
        }

        [Fact]
        public void Expect_to_find_attributes()
        {
            var classInfo = fileInfo.Classes.First();
            var attributeInfo = classInfo.Attributes.First();

            classInfo.Attributes.Count().ShouldEqual(1);
            attributeInfo.Name.ShouldEqual("AttributeInfo");
            attributeInfo.FullName.ShouldEqual("Typezor.Tests.CodeModel.Support.AttributeInfoAttribute");
        }

        [Fact]
        public void Expect_to_find_base_class()
        {
            var classInfo = fileInfo.Classes.First();
            var baseClassInfo = classInfo.BaseClass;
            baseClassInfo.Name.ShouldEqual("BaseRecordInfo");

            baseClassInfo.Properties.Count().ShouldEqual(2);
            var propertyInfo = baseClassInfo.Properties.First(p => p.Name == "PublicBaseProperty");
            propertyInfo.Name.ShouldEqual("PublicBaseProperty");

            var propertyInfo1 = baseClassInfo.Properties.First(p => p.Name == "ImmutableBaseProperty");
            propertyInfo1.Name.ShouldEqual("ImmutableBaseProperty");
        }

        [Fact]
        public void Expect_not_to_find_object_base_class()
        {
            var classInfo = fileInfo.Classes.First(c => c.Name == "BaseRecordInfo");

            classInfo.BaseClass.ShouldBeNull();
        }

        [Fact]
        public void Expect_to_find_interfaces()
        {
            var classInfo = fileInfo.Classes.First();
            var interfaceInfo = classInfo.Interfaces.First(p => p.Name == "IInterfaceInfo");
            var propertyInfo = interfaceInfo.Properties.First();

            classInfo.Interfaces.Count().ShouldEqual(2);
            interfaceInfo.Name.ShouldEqual("IInterfaceInfo");

            interfaceInfo.Properties.Count().ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicProperty");
        }

        [Fact]
        public void Expect_non_generic_class_not_to_be_generic()
        {
            var classInfo = fileInfo.Classes.First();

            classInfo.IsGeneric.ShouldBeFalse();
            classInfo.TypeParameters.Count().ShouldEqual(0);
        }

        [Fact]
        public void Expect_generic_class_to_be_generic()
        {
            var classInfo = fileInfo.Classes.First(i => i.Name == "GenericRecordInfo");
            var genericTypeArgument = classInfo.TypeParameters.First();

            classInfo.IsGeneric.ShouldBeTrue();
            classInfo.TypeParameters.Count().ShouldEqual(1);
            genericTypeArgument.Name.ShouldEqual("T");
        }

        [Fact]
        public void Expect_to_find_public_constants()
        {
            var classInfo = fileInfo.Classes.First();
            var constantInfo = classInfo.Constants.First();

            classInfo.Constants.Count().ShouldEqual(1);
            constantInfo.Name.ShouldEqual("PublicConstant");
        }

        [Fact]
        public void Expect_to_find_public_delegates()
        {
            var classInfo = fileInfo.Classes.First();
            var delegateInfo = classInfo.Delegates.First();

            classInfo.Delegates.Count().ShouldEqual(1);
            delegateInfo.Name.ShouldEqual("PublicDelegate");
        }

        [Fact]
        public void Expect_to_find_public_events()
        {
            var classInfo = fileInfo.Classes.First();
            var delegateInfo = classInfo.Events.First();

            classInfo.Events.Count().ShouldEqual(1);
            delegateInfo.Name.ShouldEqual("PublicEvent");
        }

        [Fact]
        public void Expect_to_find_public_fields()
        {
            var classInfo = fileInfo.Classes.First();
            var fieldInfo = classInfo.Fields.First();

            classInfo.Fields.Count().ShouldEqual(1);
            fieldInfo.Name.ShouldEqual("PublicField");
        }

        [Fact]
        public void Expect_to_find_public_methods()
        {
            var classInfo = fileInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "PublicMethod");

            classInfo.Methods.Count().ShouldEqual(8);
            methodInfo.Name.ShouldEqual("PublicMethod");
        }

        [Fact]
        public void Expect_to_find_public_properties()
        {
            var classInfo = fileInfo.Classes.First();
            classInfo.Properties.Count().ShouldEqual(2);

            var propertyInfo = classInfo.Properties.Single(p => p.Name == "PublicProperty");
            propertyInfo.Name.ShouldEqual("PublicProperty");

            var propertyInfo1 = classInfo.Properties.Single(p => p.Name == "ImmutableProperty");
            propertyInfo1.Name.ShouldEqual("ImmutableProperty");
        }

        [Fact]
        public void Expect_to_find_nested_public_records()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedClassInfo = classInfo.NestedClasses.Single(p => p.Name == "NestedRecordInfo");
            nestedClassInfo.IsRecord.ShouldBeTrue();
            var propertyInfo = nestedClassInfo.Properties.First();

            classInfo.NestedClasses.Count().ShouldEqual(2);
            nestedClassInfo.Name.ShouldEqual("NestedRecordInfo");

            nestedClassInfo.Properties.Count().ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicNestedProperty");
        }

        [Fact]
        public void Expect_to_find_nested_public_enums()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedEnumInfo = classInfo.NestedEnums.First();
            var valueInfo = nestedEnumInfo.Values.First();

            classInfo.NestedEnums.Count().ShouldEqual(1);
            nestedEnumInfo.Name.ShouldEqual("NestedEnumInfo");

            nestedEnumInfo.Values.Count().ShouldEqual(1);
            valueInfo.Name.ShouldEqual("NestedValue");
        }

        [Fact]
        public void Expect_to_find_nested_public_interfaces()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedInterfaceInfo = classInfo.NestedInterfaces.First();
            var propertyInfo = nestedInterfaceInfo.Properties.First();

            classInfo.NestedInterfaces.Count().ShouldEqual(1);
            nestedInterfaceInfo.Name.ShouldEqual("INestedInterfaceInfo");

            nestedInterfaceInfo.Properties.Count().ShouldEqual(1);
            propertyInfo.Name.ShouldEqual("PublicNestedProperty");
        }

        [Fact]
        public void Expect_to_find_containing_class_on_nested_class()
        {
            var classInfo = fileInfo.Classes.First();
            var nestedClassInfo = classInfo.NestedClasses.First();
            var containingClassInfo = nestedClassInfo.ContainingClass;

            containingClassInfo.Name.ShouldEqual("RecordInfo");
        }

        [Fact]
        public void Expect_not_to_find_containing_class_on_top_level_class()
        {
            var classInfo = fileInfo.Classes.First();
            var containingClassInfo = classInfo.ContainingClass;

            containingClassInfo.ShouldBeNull();
        }

        [Fact]
        public void Expect_generic_baseclass_to_have_type_arguments()
        {
            var classInfo = fileInfo.Classes.First(m => m.Name == "InheritGenericRecordInfo");
            var genericTypeArgument = classInfo.BaseClass.TypeArguments.First();

            classInfo.BaseClass.IsGeneric.ShouldBeTrue();
            classInfo.BaseClass.TypeArguments.Count().ShouldEqual(1);

            genericTypeArgument.Name.ShouldEqual("string");


            var genericTypeParameter = classInfo.BaseClass.TypeParameters.First();
            classInfo.BaseClass.TypeParameters.Count().ShouldEqual(1);
            genericTypeParameter.Name.ShouldEqual("T");

        }
    }
}