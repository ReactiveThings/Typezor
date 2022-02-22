using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Extensions.WebApi;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.Extensions
{

    [Trait("Extensions", "WebApi"), Collection(nameof(RoslynFixture))]
    public class RoslynWebApiRouteExtensionsTests : WebApiRouteExtensionsTests
    {
        public RoslynWebApiRouteExtensionsTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class WebApiRouteExtensionsTests : TestBase
    {
        private readonly File fileInfo;
        private readonly File routeLessControllerInfo;

        protected WebApiRouteExtensionsTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"Extensions\Support\RouteController.cs", 
                @"Support\RouteAttribute.cs",
                @"Support\AcceptVerbsAttribute.cs",
                @"Support\FromBodyAttribute.cs",
                @"Support\HttpDeleteAttribute.cs",
                @"Support\HttpGetAttribute.cs",
                @"Support\HttpPostAttribute.cs",
                @"Support\HttpPutAttribute.cs",
                @"Support\RoutePrefixAttribute.cs");
            routeLessControllerInfo = GetFile(
                @"Extensions\Support\RouteLessController.cs", 
                @"Support\RouteAttribute.cs",
                @"Support\AcceptVerbsAttribute.cs",
                @"Support\FromBodyAttribute.cs",
                @"Support\HttpDeleteAttribute.cs",
                @"Support\HttpGetAttribute.cs",
                @"Support\HttpPostAttribute.cs",
                @"Support\HttpPutAttribute.cs",
                @"Support\RoutePrefixAttribute.cs");
        }

        [Fact]
        public void Expect_to_route_on_routless_Controller_with_methodattribute()
        {
            var classInfo = routeLessControllerInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "Test");

            methodInfo.Url().ShouldEqual("api/RouteLess/${id}");
        }

        [Fact]
        public void Expect_to_route_on_routless_Controller_without_methodattribute()
        {
            var classInfo = routeLessControllerInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "Test2");

            methodInfo.Url().ShouldEqual("api/RouteLess/${id}");
        }

        [Fact]
        public void Expect_to_route_on_routless_Controller_without_methodattribute_and_inputparam()
        {
            var classInfo = routeLessControllerInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "Test3");

            methodInfo.Url().ShouldEqual("api/RouteLess/");
        }

        [Fact]
        public void Expect_to_route_on_routless_Controller_with_methodattribute_and_inputparam_custom_route()
        {
            var classInfo = routeLessControllerInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "Test");

            methodInfo.Url("api/{controller}/{action}/{id?}").ShouldEqual("api/RouteLess/test/${id}");
        }


        [Fact]
        public void Expect_to_find_parameters_on_wildcard_route_url()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "WildcardRoute");

            methodInfo.Url().ShouldEqual("api/${encodeURIComponent(key)}");
        }

        [Fact]
        public void Expect_to_find_url_on_named_route()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "NamedRoute");

            methodInfo.Url().ShouldEqual("api/${id}");
        }

        [Fact]
        public void Expect_to_find_url_on_httpget_route()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "HttpGetRoute");

            methodInfo.Url().ShouldEqual("api/${id}");
        }
        
        [Fact]
        public void Expect_request_data_to_ignore_route_parameters()
        {
            var classInfo = fileInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "NamedRoute");

            methodInfo.RequestData().ShouldEqual("null");
        }

        [Fact]
        public void Expect_to_find_url_on_action_without_route_attribute_and_id()
        {
            var classInfo = fileInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "NoRouteWithId");

            var result = methodInfo.Url();
            result.ShouldEqual("api/Route/${id}");
        }

        [Fact]
        public void Expect_to_find_url_on_action_without_route_attribute()
        {
            var classInfo = fileInfo.Classes.First();
            var methodInfo = classInfo.Methods.First(p => p.Name == "NoRoute");

            var result = methodInfo.Url();
            result.ShouldEqual("api/Route/");
        }
    }
}