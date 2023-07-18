﻿using System.Linq;
using Should;
using Typezor.CodeModel;
using Typezor.Tests.CodeModel.Support;
using Typezor.Tests.TestInfrastructure;
using Xunit;

namespace Typezor.Tests.CodeModel
{
    [Trait("CodeModel", "Attribute"), Collection(nameof(RoslynFixture))]
    public class RoslynAttributeTests : ClassTests
    {
        public RoslynAttributeTests(RoslynFixture fixture) : base(fixture)
        {
        }
    }

    public abstract class AttributeTests : TestBase
    {
        private readonly Class classInfo;

        protected AttributeTests(ITestFixture fixture) : base(fixture)
        {
            var fileInfo = GetFile(@"CodeModel\Support\AttributeInfo.cs");
            classInfo = fileInfo.Classes.First(c => c.Name == nameof(AttributeTestClass));
        }

        [Fact]
        public void Expect_name_to_match_attribute_name()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "NoParameters");
            var attributeInfo = propertyInfo.Attributes.First();

            propertyInfo.Attributes.Count().ShouldEqual(1);
            attributeInfo.Name.ShouldEqual("AttributeInfo");
            attributeInfo.FullName.ShouldEqual("Typezor.Tests.CodeModel.Support.AttributeInfoAttribute");
        }

        [Fact]
        public void Expect_attributes_with_no_parameters_to_have_an_empty_value()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "NoParameters");
            var attributeInfo = propertyInfo.Attributes.First();
            
            attributeInfo.Value.ShouldBeNull();
        }

        [Fact]
        public void Expect_attributes_with_string_parameter_to_have_a_string_value()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "StringParameter");
            var attributeInfo = propertyInfo.Attributes.First();
            
            attributeInfo.Value.ShouldEqual("parameter");
        }

        [Fact]
        public void Expect_attributes_with_int_parameter_to_have_an_integer_value()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "IntParameter");
            var attributeInfo = propertyInfo.Attributes.First();
            
            attributeInfo.Value.ShouldEqual("1");
        }

        [Fact]
        public void Expect_attributes_with_int_and_named_parameter_to_have_a_proper_value()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "IntAndNamedParameter");
            var attributeInfo = propertyInfo.Attributes.First();
            
            attributeInfo.Value.ShouldEqual("2, Parameter = \"parameter\"");
        }

        [Fact]
        public void Expect_attributes_with_params_parameter_to_have_a_proper_value()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "ParamsParameter");
            var attributeInfo = propertyInfo.Attributes.First();

            attributeInfo.Value.ShouldEqual("\"parameter1\", \"parameter2\"");
        }

        [Fact]
        public void Expect_attributes_with_string_and_params_parameter_to_have_a_proper_value()
        {
            var propertyInfo = classInfo.Properties.First(p => p.Name == "IntAndParamsParameter");
            var attributeInfo = propertyInfo.Attributes.First();

            attributeInfo.Value.ShouldEqual("1, \"parameter\"");
        }
    }
}