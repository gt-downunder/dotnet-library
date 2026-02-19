using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class ObjectExTests : BaseExtensionTest
    {
        [TestMethod]
        public void IsNumeric_IntValue_ReturnsTrue() => ((object)42).IsNumeric().Should().BeTrue();

        [TestMethod]
        public void IsNumeric_DoubleValue_ReturnsTrue() => ((object)3.14).IsNumeric().Should().BeTrue();

        [TestMethod]
        public void IsNumeric_NumericString_ReturnsTrue() => ((object)"123.45").IsNumeric().Should().BeTrue();

        [TestMethod]
        public void IsNumeric_NonNumericString_ReturnsFalse() => ((object)"hello").IsNumeric().Should().BeFalse();

        [TestMethod]
        public void IsNumeric_Null_ReturnsFalse() => ((object?)null).IsNumeric().Should().BeFalse();

        [TestMethod]
        public void IsNumeric_BoolValue_ReturnsFalse() => ((object)true).IsNumeric().Should().BeFalse();

        [TestMethod]
        public void ToNullableDouble_ValidString_ReturnsValue() =>
            ((object)"3.14").ToNullableDouble().Should().BeApproximately(3.14, 0.001);

        [TestMethod]
        public void ToNullableDouble_Invalid_ReturnsNull() =>
            ((object)"abc").ToNullableDouble().Should().BeNull();

        [TestMethod]
        public void ToNullableDouble_Null_ReturnsNull() =>
            ((object?)null).ToNullableDouble().Should().BeNull();

        [TestMethod]
        public void ToNullableInteger_ValidString_ReturnsValue() =>
            ((object)"42").ToNullableInteger().Should().Be(42);

        [TestMethod]
        public void ToNullableInteger_Invalid_ReturnsNull() =>
            ((object)"xyz").ToNullableInteger().Should().BeNull();

        [TestMethod]
        public void ToNullableBoolean_TrueString_ReturnsTrue() =>
            ((object)"true").ToNullableBoolean().Should().BeTrue();

        [TestMethod]
        public void ToNullableBoolean_Invalid_ReturnsNull() =>
            ((object)"maybe").ToNullableBoolean().Should().BeNull();

        [TestMethod]
        public void ToNullableDateTime_ValidDate_ReturnsValue()
        {
            DateTime? result = ((object)"2024-01-15").ToNullableDateTime();
            result.Should().NotBeNull();
            result!.Value.Year.Should().Be(2024);
        }

        [TestMethod]
        public void ToNullableDateTime_WithFormat_ReturnsValue()
        {
            DateTime? result = ((object)"15/01/2024").ToNullableDateTime("dd/MM/yyyy");
            result.Should().NotBeNull();
            result!.Value.Day.Should().Be(15);
        }

        [TestMethod]
        public void ToNullableDateTime_Invalid_ReturnsNull() =>
            ((object)"not-a-date").ToNullableDateTime().Should().BeNull();

        [TestMethod]
        public void ToStringContent_StringObject_CreatesContent()
        {
            var content = "hello".ToStringContent("text/plain");
            content.Should().NotBeNull();
            content.Headers.ContentType!.MediaType.Should().Be("text/plain");
        }

        [TestMethod]
        public void ToStringContent_NonStringObject_SerializesToJson()
        {
            var obj = new { Name = "test" };
            var content = obj.ToStringContent("application/json");
            content.Should().NotBeNull();
            content.Headers.ContentType!.MediaType.Should().Be("application/json");
        }
    }
}

