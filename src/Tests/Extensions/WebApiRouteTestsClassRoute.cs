using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Extensions.WebApi;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.Extensions
{

    [Trait("Extensions", "WebApi"), Collection(nameof(RoslynFixture))]
    public class RoslynWebApiRouteClassRouteExtensionsTests : WebApiRouteClassRouteExtensionsTests
    {
        public RoslynWebApiRouteClassRouteExtensionsTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class WebApiRouteClassRouteExtensionsTests : TestBase
    {
        private readonly File fileInfo;
        private readonly File inheritedFileInfo;


        protected WebApiRouteClassRouteExtensionsTests(ITestFixture fixture) : base(fixture)
        {
            fileInfo = GetFile(@"Extensions\Support\RouteControllerWithDefaultRoute.cs",
                @"Support\RouteAttribute.cs",
                @"Support\AcceptVerbsAttribute.cs",
                @"Support\FromBodyAttribute.cs",
                @"Support\HttpDeleteAttribute.cs",
                @"Support\HttpGetAttribute.cs",
                @"Support\HttpPostAttribute.cs",
                @"Support\HttpPutAttribute.cs",
                @"Support\RoutePrefixAttribute.cs");
            inheritedFileInfo = GetFile(
                @"Extensions\Support\InheritedController.cs",
                @"Extensions\Support\RouteControllerWithDefaultRoute.cs",
                @"Extensions\Support\BaseController.cs",
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
        public void Expect_to_find_parameters_on_wildcard_route_url()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "WildcardRoute");
            var result = methodInfo.Url();
            result.ShouldEqual("api/RouteControllerWithDefaultRoute/${encodeURIComponent(key)}");
        }

        [Fact]
        public void Expect_to_find_url_on_named_route()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "NamedRoute");
            var result = methodInfo.Url();
            result.ShouldEqual("api/RouteControllerWithDefaultRoute/${id}");
        }

        [Fact]
        public void Expect_to_find_url_on_route_in_http_attribute()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "RouteInHttpAttribute");

            methodInfo.Url().ShouldEqual("api/RouteControllerWithDefaultRoute/${id}");
        }

        [Fact]
        public void Expect_to_find_url_on_subroute_in_http_attribute()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "SubRouteInHttpAttribute");

            methodInfo.Url().ShouldEqual("api/RouteControllerWithDefaultRoute/sub/${id}");
        }

        [Fact]
        public void Expect_to_find_url_on_in_httpget_action_attribute()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController"); ;
            var methodInfo = classInfo.Methods.First(p => p.Name == "ActionTestInheritedClassController");

            var result = methodInfo.Url();
            result.ShouldEqual("api/RouteControllerWithDefaultRoute/actionTestInheritedClassController");
        }
        [Fact]
        public void Expect_to_find_url_on_BaseController_HttpGet_Parameter()
        {
            var classInfo = inheritedFileInfo.Classes.First(p => p.Name == "InheritedController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "RoutePrefixFromBaseHttpGetWithParameter");

            var result = methodInfo.Url();
            result.ShouldEqual("api/Inherited/inherited/${id}");
        }
        

        [Fact]
        public void Expect_to_find_url_on_in_httpget_action_withparameter()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "ActionTestInheritedClassControllerPostWithParameter");

            var result = methodInfo.Url();
            result.ShouldEqual("api/RouteControllerWithDefaultRoute/actionTestInheritedClassControllerPostWithParameter/${id}");
        }

        [Fact]
        public void Expect_to_find_url_on_in_httppost_action_withparameter()
        {
            var classInfo = fileInfo.Classes.First(p => p.Name == "RouteControllerWithDefaultRouteController");
            var methodInfo = classInfo.Methods.First(p => p.Name == "ActionTestInheritedClassControllerPostWithParameter");

            var result = methodInfo.Url();
            result.ShouldEqual("api/RouteControllerWithDefaultRoute/actionTestInheritedClassControllerPostWithParameter/${id}");
        }

        

    }
}