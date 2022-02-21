using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;
using Xunit;

namespace Typezor.Tests.Helpers
{
    [Trait("Helpers", "CamelCase")]
    public class CamelCaseTests
    {
        [Fact]
        public void Expect_strings_to_be_camel_cased_correctly()
        {
            // Tests from Json.NET
            // https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json.Tests/Utilities/StringUtilsTests.cs
            Typezor.CodeModel.Helpers.CamelCase("URLValue").ShouldEqual("urlValue");
            Typezor.CodeModel.Helpers.CamelCase("URL").ShouldEqual("url");
            Typezor.CodeModel.Helpers.CamelCase("ID").ShouldEqual("id");
            Typezor.CodeModel.Helpers.CamelCase("I").ShouldEqual("i");
            Typezor.CodeModel.Helpers.CamelCase("").ShouldEqual("");
            Typezor.CodeModel.Helpers.CamelCase(null).ShouldEqual(null);
            Typezor.CodeModel.Helpers.CamelCase("Person").ShouldEqual("person");
            Typezor.CodeModel.Helpers.CamelCase("iPhone").ShouldEqual("iPhone");
            Typezor.CodeModel.Helpers.CamelCase("IPhone").ShouldEqual("iPhone");
            Typezor.CodeModel.Helpers.CamelCase("I Phone").ShouldEqual("i Phone");
            Typezor.CodeModel.Helpers.CamelCase("I  Phone").ShouldEqual("i  Phone");
            Typezor.CodeModel.Helpers.CamelCase(" IPhone").ShouldEqual(" IPhone");
            Typezor.CodeModel.Helpers.CamelCase(" IPhone ").ShouldEqual(" IPhone ");
            Typezor.CodeModel.Helpers.CamelCase("IsCIA").ShouldEqual("isCIA");
            Typezor.CodeModel.Helpers.CamelCase("VmQ").ShouldEqual("vmQ");
            Typezor.CodeModel.Helpers.CamelCase("Xml2Json").ShouldEqual("xml2Json");
            Typezor.CodeModel.Helpers.CamelCase("SnAkEcAsE").ShouldEqual("snAkEcAsE");
            Typezor.CodeModel.Helpers.CamelCase("SnA__kEcAsE").ShouldEqual("snA__kEcAsE");
            Typezor.CodeModel.Helpers.CamelCase("SnA__ kEcAsE").ShouldEqual("snA__ kEcAsE");
            Typezor.CodeModel.Helpers.CamelCase("already_snake_case_ ").ShouldEqual("already_snake_case_ ");
            Typezor.CodeModel.Helpers.CamelCase("IsJSONProperty").ShouldEqual("isJSONProperty");
            Typezor.CodeModel.Helpers.CamelCase("SHOUTING_CASE").ShouldEqual("shoutinG_CASE");
            Typezor.CodeModel.Helpers.CamelCase("9999-12-31T23:59:59.9999999Z").ShouldEqual("9999-12-31T23:59:59.9999999Z");
            Typezor.CodeModel.Helpers.CamelCase("Hi!! This is text. Time to test.").ShouldEqual("hi!! This is text. Time to test.");
        }
    }
}
