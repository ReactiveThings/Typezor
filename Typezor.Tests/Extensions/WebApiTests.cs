using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Extensions.WebApi;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.Extensions
{

    [Trait("Extensions", "WebApi"), Collection(nameof(RoslynFixture))]
    public class RoslynWebApiExtensionsTests : WebApiExtensionsTests
    {
        public RoslynWebApiExtensionsTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class WebApiExtensionsTests : TestBase
    {
        private readonly File fileInfo;

        protected WebApiExtensionsTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"Extensions\Support\HttpMethodController.cs", 
                @"CodeModel\Support\AttributeInfo.cs",
                @"Support\AcceptVerbsAttribute.cs");
        }

        [Fact]
        public void Expect_httpmethod_to_match_convension_name()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "HttpMethodController");
            var getInfo = classInfo.Methods.First(p => p.Name == "Get");
            var getAllMethod = classInfo.Methods.First(p => p.Name == "GetAll");
            var listAllMethod = classInfo.Methods.First(p => p.Name == "ListAll");

            getInfo.HttpMethod().ShouldEqual("get");
            getAllMethod.HttpMethod().ShouldEqual("get");
            listAllMethod.HttpMethod().ShouldEqual("post");
        }

        [Fact]
        public void Expect_httpmethod_to_match_http_attribute()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "HttpMethodController");
            var getMethod = classInfo.Methods.First(p => p.Name == "GetHttpAttibute");

            getMethod.HttpMethod().ShouldEqual("post");
        }

        [Fact]
        public void Expect_httpmethod_to_match_acceptverbs_attribute()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "HttpMethodController");
            var getMethod = classInfo.Methods.First(p => p.Name == "GetAcceptVerbsAttribute");
            var getMultipleMethod1 = classInfo.Methods.First(p => p.Name == "GetMultipleAcceptVerbsAttribute1");
            var getMultipleMethod2 = classInfo.Methods.First(p => p.Name == "GetMultipleAcceptVerbsAttribute2");

            getMethod.HttpMethod().ShouldEqual("head");
            getMultipleMethod1.HttpMethod().ShouldEqual("post");
            getMultipleMethod2.HttpMethod().ShouldEqual("head");
        }
    }
}