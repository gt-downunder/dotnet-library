using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class XmlSerializerExTests : BaseExtensionTest
    {
        public class TestEntity
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        [TestMethod]
        public void Serialize_ValidEntity_ReturnsXmlString()
        {
            var entity = new TestEntity { Name = "Test", Value = 42 };
            string xml = entity.Serialize();

            xml.Should().Contain("<Name>Test</Name>");
            xml.Should().Contain("<Value>42</Value>");
        }

        [TestMethod]
        public void Serialize_NullEntity_ThrowsArgumentNullException()
        {
            TestEntity entity = null!;
            Func<string> act = () => entity.Serialize();
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Deserialize_ValidXml_ReturnsEntity()
        {
            var entity = new TestEntity { Name = "Hello", Value = 7 };
            string xml = entity.Serialize();
            TestEntity? result = xml.Deserialize<TestEntity>();

            result.Should().NotBeNull();
            result!.Name.Should().Be("Hello");
            result.Value.Should().Be(7);
        }

        [TestMethod]
        public void Deserialize_NullOrWhitespace_ReturnsDefault()
        {
            TestEntity? result = "".Deserialize<TestEntity>();
            result.Should().BeNull();
        }

        [TestMethod]
        public void RoundTrip_ComplexEntity_PreservesData()
        {
            var original = new TestEntity { Name = "Round Trip", Value = 999 };
            string xml = original.Serialize();
            TestEntity? deserialized = xml.Deserialize<TestEntity>();

            deserialized.Should().NotBeNull();
            deserialized!.Name.Should().Be(original.Name);
            deserialized.Value.Should().Be(original.Value);
        }
    }
}

