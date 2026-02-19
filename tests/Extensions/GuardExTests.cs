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

        // --- Numeric Guards ---

        [TestMethod]
        public void ThrowIfNegative_PositiveValue_ReturnsValue() => 5.ThrowIfNegative("n").Should().Be(5);

        [TestMethod]
        public void ThrowIfNegative_Zero_ReturnsZero() => 0.ThrowIfNegative("n").Should().Be(0);

        [TestMethod]
        public void ThrowIfNegative_NegativeValue_ThrowsArgumentOutOfRangeException()
        {
            Func<int> act = () => (-1).ThrowIfNegative("n");
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        }

        [TestMethod]
        public void ThrowIfNegative_DoubleNegative_Throws()
        {
            Func<double> act = () => (-0.5).ThrowIfNegative("d");
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ThrowIfNegativeOrZero_PositiveValue_ReturnsValue() => 10.ThrowIfNegativeOrZero("n").Should().Be(10);

        [TestMethod]
        public void ThrowIfNegativeOrZero_Zero_Throws()
        {
            Func<int> act = () => 0.ThrowIfNegativeOrZero("n");
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        }

        [TestMethod]
        public void ThrowIfNegativeOrZero_Negative_Throws()
        {
            Func<int> act = () => (-5).ThrowIfNegativeOrZero("n");
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ThrowIfZero_NonZero_ReturnsValue() => 42.ThrowIfZero("n").Should().Be(42);

        [TestMethod]
        public void ThrowIfZero_Zero_Throws()
        {
            Func<int> act = () => 0.ThrowIfZero("n");
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        }

        [TestMethod]
        public void ThrowIfZero_NegativeValue_ReturnsValue() => (-3).ThrowIfZero("n").Should().Be(-3);

        [TestMethod]
        public void ThrowIfOutOfRange_InRange_ReturnsValue() => 5.ThrowIfOutOfRange(1, 10, "n").Should().Be(5);

        [TestMethod]
        public void ThrowIfOutOfRange_AtMin_ReturnsValue() => 1.ThrowIfOutOfRange(1, 10, "n").Should().Be(1);

        [TestMethod]
        public void ThrowIfOutOfRange_AtMax_ReturnsValue() => 10.ThrowIfOutOfRange(1, 10, "n").Should().Be(10);

        [TestMethod]
        public void ThrowIfOutOfRange_BelowMin_Throws()
        {
            Func<int> act = () => 0.ThrowIfOutOfRange(1, 10, "n");
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        }

        [TestMethod]
        public void ThrowIfOutOfRange_AboveMax_Throws()
        {
            Func<int> act = () => 11.ThrowIfOutOfRange(1, 10, "n");
            act.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("n");
        }

        // --- Predicate Guard ---

        [TestMethod]
        public void ThrowIf_PredicateFalse_ReturnsValue() =>
            42.ThrowIf(x => x > 100, "Too large", "n").Should().Be(42);

        [TestMethod]
        public void ThrowIf_PredicateTrue_ThrowsArgumentException()
        {
            Func<int> act = () => 150.ThrowIf(x => x > 100, "Too large", "n");
            act.Should().Throw<ArgumentException>().WithParameterName("n")
                .WithMessage("Too large*");
        }

        [TestMethod]
        public void ThrowIf_NullPredicate_ThrowsArgumentNullException()
        {
            Func<int> act = () => 42.ThrowIf(null!, "msg", "n");
            act.Should().Throw<ArgumentNullException>();
        }

        // --- Regex Format Guard ---

        [TestMethod]
        public void ThrowIfInvalidFormat_ValidFormat_ReturnsValue() =>
            "ABC-123".ThrowIfInvalidFormat(@"^[A-Z]+-\d+$", "code").Should().Be("ABC-123");

        [TestMethod]
        public void ThrowIfInvalidFormat_InvalidFormat_ThrowsArgumentException()
        {
            Func<string> act = () => "invalid".ThrowIfInvalidFormat(@"^\d+$", "num");
            act.Should().Throw<ArgumentException>().WithParameterName("num");
        }

        [TestMethod]
        public void ThrowIfInvalidFormat_NullValue_ThrowsArgumentNullException()
        {
            string? value = null;
            Func<string> act = () => value.ThrowIfInvalidFormat(@"\d+", "p");
            act.Should().Throw<ArgumentNullException>();
        }
    }
}

