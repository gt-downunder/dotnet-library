using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class GuardExTests : BaseExtensionTest
    {
        [TestMethod]
        public void ThrowIfNull_NonNull_ReturnsValue()
        {
            string value = "hello";
            value.ThrowIfNull("param").Should().Be("hello");
        }

        [TestMethod]
        public void ThrowIfNull_Null_ThrowsArgumentNullException()
        {
            string? value = null;
            Func<string> act = () => value.ThrowIfNull("myParam");
            act.Should().Throw<ArgumentNullException>().WithParameterName("myParam");
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_ValidString_ReturnsValue() => "test".ThrowIfNullOrWhiteSpace("p").Should().Be("test");

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_Null_ThrowsArgumentNullException()
        {
            string? value = null;
            Func<string> act = () => value.ThrowIfNullOrWhiteSpace("p");
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_Whitespace_ThrowsArgumentException()
        {
            Func<string> act = () => "  ".ThrowIfNullOrWhiteSpace("p");
            act.Should().Throw<ArgumentException>().WithParameterName("p");
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_Empty_ThrowsArgumentException()
        {
            Func<string> act = () => "".ThrowIfNullOrWhiteSpace("p");
            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ThrowIfDefault_NonDefault_ReturnsValue() => 42.ThrowIfDefault("p").Should().Be(42);

        [TestMethod]
        public void ThrowIfDefault_DefaultInt_ThrowsArgumentException()
        {
            Func<int> act = () => 0.ThrowIfDefault("p");
            act.Should().Throw<ArgumentException>().WithParameterName("p");
        }

        [TestMethod]
        public void ThrowIfDefault_DefaultGuid_ThrowsArgumentException()
        {
            Func<Guid> act = () => Guid.Empty.ThrowIfDefault("id");
            act.Should().Throw<ArgumentException>().WithParameterName("id");
        }

        [TestMethod]
        public void ThrowIfEmpty_NonEmptyCollection_ReturnsCollection()
        {
            int[] items = [1, 2, 3];
            items.ThrowIfEmpty("items").Should().BeEquivalentTo([1, 2, 3]);
        }

        [TestMethod]
        public void ThrowIfEmpty_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int>? items = null;
            Func<IEnumerable<int>> act = () => items.ThrowIfEmpty("items");
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ThrowIfEmpty_EmptyCollection_ThrowsArgumentException()
        {
            Func<IEnumerable<int>> act = () => Array.Empty<int>().ThrowIfEmpty("items");
            act.Should().Throw<ArgumentException>().WithParameterName("items");
        }
    }
}

