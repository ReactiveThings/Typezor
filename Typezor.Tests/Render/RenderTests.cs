﻿
//using Typezor.Tests.TestInfrastructure;
//using Xunit;

//namespace Typezor.Tests.Render
//{

//    [Trait("Render", "Roslyn"), Collection(nameof(RoslynFixture))]
//    public class RoslynRenderTests : RenderTests
//    {
//        public RoslynRenderTests(RoslynFixture fixture) : base(fixture)
//        {
//        }
//    }

//    public abstract class RenderTests : TestBase
//    {
//        protected RenderTests(ITestFixture fixture) : base(fixture)
//        {
//        }

//        private void Assert<TClass>()
//        {
//            var type = typeof(TClass);
//            var nsParts = type.FullName.Remove(0, 11).Split('.');

//            var path = string.Join(@"\", nsParts);

//            var template = new Template(GetProjectItem(path + ".tstemplate"));
//            var file = GetFile(path + ".cs");
//            var result = GetFileContents(path + ".result");

//            var output = template.Render(file);

//            output.Single().Content.ShouldEqual(result);
//        }

//        //[Fact]
//        //public void Expect_webapi_controller_to_angular_service_to_render_correctly()
//        //{
//        //    Assert<WebApiController.WebApiController>();
//        //}

//        //[Fact]
//        //public void Expect_routed_webapi_controller_to_render_correctly()
//        //{
//        //    Assert<BooksController>();
//        //}
//    }
//}
