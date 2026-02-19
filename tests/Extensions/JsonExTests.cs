using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class JsonExTests : BaseExtensionTest
    {
        private record TestDto(string Name, int Value);

        [TestMethod]
        public void ToJson_Object_ReturnsJsonString()
        {
            var dto = new TestDto("Test", 42);
            string json = dto.ToJson();

            json.Should().Contain("\"Name\"").And.Contain("\"Value\"");
        }

        [TestMethod]
        public void ToJson_Indented_ReturnsFormattedJson()
        {
            var dto = new TestDto("Test", 42);
            string json = dto.ToJson(indented: true);

            json.Should().Contain("\n");
        }

        [TestMethod]
        public void FromJson_ValidJson_ReturnsObject()
        {
            string json = "{\"Name\":\"Hello\",\"Value\":99}";
            TestDto? result = json.FromJson<TestDto>();

            result.Should().NotBeNull();
            result!.Name.Should().Be("Hello");
            result.Value.Should().Be(99);
        }

        [TestMethod]
        public void FromJson_CaseInsensitive_MatchesDifferentCasing()
        {
            string json = "{\"name\":\"Hello\",\"value\":99}";
            TestDto? result = json.FromJson<TestDto>(caseInsensitive: true);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Hello");
        }

        [TestMethod]
        public void FromJson_NullJson_ThrowsArgumentNullException()
        {
            string json = null!;
            Func<TestDto?> act = () => json.FromJson<TestDto>();
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void TryFromJson_ValidJson_ReturnsTrueAndResult()
        {
            string json = "{\"Name\":\"Test\",\"Value\":1}";
            bool success = json.TryFromJson<TestDto>(out TestDto? result);

            success.Should().BeTrue();
            result.Should().NotBeNull();
            result!.Name.Should().Be("Test");
        }

        [TestMethod]
        public void TryFromJson_InvalidJson_ReturnsFalse()
        {
            bool success = "not-json".TryFromJson<TestDto>(out TestDto? result);

            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [TestMethod]
        public void TryFromJson_NullOrWhitespace_ReturnsFalse()
        {
            bool success = "".TryFromJson<TestDto>(out TestDto? result);

            success.Should().BeFalse();
            result.Should().BeNull();
        }

        [TestMethod]
        public void RoundTrip_SerializeAndDeserialize_PreservesData()
        {
            var original = new TestDto("RoundTrip", 123);
            string json = original.ToJson();
            TestDto? deserialized = json.FromJson<TestDto>();

            deserialized.Should().Be(original);
        }
    }
}

