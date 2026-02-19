using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class NumericExTests : BaseExtensionTest
    {
        [TestMethod]
        public void IsZero_Zero_ReturnsTrue() => 0.IsZero().Should().BeTrue();

        [TestMethod]
        public void IsZero_Positive_ReturnsFalse() => 5.IsZero().Should().BeFalse();

        [TestMethod]
        public void IsZero_Negative_ReturnsFalse() => (-3).IsZero().Should().BeFalse();

        [TestMethod]
        public void IsZero_DoubleZero_ReturnsTrue() => 0.0.IsZero().Should().BeTrue();

        [TestMethod]
        public void IsPositive_PositiveInt_ReturnsTrue() => 42.IsPositive().Should().BeTrue();

        [TestMethod]
        public void IsPositive_Zero_ReturnsFalse() => 0.IsPositive().Should().BeFalse();

        [TestMethod]
        public void IsPositive_Negative_ReturnsFalse() => (-1).IsPositive().Should().BeFalse();

        [TestMethod]
        public void IsPositive_PositiveDouble_ReturnsTrue() => 3.14.IsPositive().Should().BeTrue();

        [TestMethod]
        public void IsNegative_NegativeInt_ReturnsTrue() => (-7).IsNegative().Should().BeTrue();

        [TestMethod]
        public void IsNegative_Zero_ReturnsFalse() => 0.IsNegative().Should().BeFalse();

        [TestMethod]
        public void IsNegative_Positive_ReturnsFalse() => 1.IsNegative().Should().BeFalse();

        [TestMethod]
        public void IsNegative_NegativeDecimal_ReturnsTrue() => (-0.5m).IsNegative().Should().BeTrue();
    }
}

