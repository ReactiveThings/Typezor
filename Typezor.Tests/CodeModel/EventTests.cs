using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.CodeModel
{

    [Trait("CodeModel", "Events"), Collection(nameof(RoslynFixture))]
    public class RoslynEventTests : EventTests
    {
        public RoslynEventTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class EventTests : TestBase
    {
        private readonly File fileInfo;

        protected EventTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"CodeModel\Support\EventInfo.cs", 
                @"CodeModel\Support\AttributeInfo.cs",
                @"CodeModel\Support\DelegateInfo.cs");
        }

        [Fact]
        public void Expect_name_to_match_property_name()
        {
            var classInfo = fileInfo.Classes.First();
            var enumInfo = classInfo.Events.First(p => p.Name == "DelegateEvent");
            
            enumInfo.Name.ShouldEqual("DelegateEvent");
            enumInfo.FullName.ShouldEqual("Typezor.Tests.CodeModel.Support.EventInfo.DelegateEvent");
            enumInfo.Parent.ShouldEqual(classInfo);
        }

        [Fact]
        public void Expect_to_find_doc_comment()
        {
            var classInfo = fileInfo.Classes.First();
            var enumInfo = classInfo.Events.First(p => p.Name == "DelegateEvent");
            enumInfo.DocComment.Summary.ShouldEqual("summary");
        }

        [Fact]
        public void Expect_to_find_attributes()
        {
            var classInfo = fileInfo.Classes.First();
            var enumInfo = classInfo.Events.First(p => p.Name == "DelegateEvent");
            var attributeInfo = enumInfo.Attributes.First();

            enumInfo.Attributes.Count().ShouldEqual(1);
            attributeInfo.Name.ShouldEqual("AttributeInfo");
            attributeInfo.FullName.ShouldEqual("Typezor.Tests.CodeModel.Support.AttributeInfoAttribute");
        }

        [Fact]
        public void Expect_generic_delegate_type_type_to_match_generic_argument()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "EventInfo");
            var eventInfo = classInfo.Events.First(e => e.Name == "GenericDelegateEvent");
            var typeInfo = eventInfo.Type;

            typeInfo.Name.ShouldEqual("GenericDelegate<string>");

            typeInfo.TypeArguments.Count().ShouldEqual(1);
            typeInfo.TypeParameters.Count().ShouldEqual(1);

            typeInfo.TypeArguments.First().Name.ShouldEqual("string");
            typeInfo.TypeParameters.First().Name.ShouldEqual("T");
        }
    }
}