using FluentAssertions;
using Grondo.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grondo.Tests.Extensions
{
    [TestClass]
    public class TimeSpanExTests : BaseExtensionTest
    {
        [TestMethod]
        public void ToHumanReadable_Zero_ReturnsZeroSeconds() => TimeSpan.Zero.ToHumanReadable().Should().Be("0 seconds");

        [TestMethod]
        public void ToHumanReadable_Seconds_ReturnsSeconds() => TimeSpan.FromSeconds(45).ToHumanReadable().Should().Be("45 seconds");

        [TestMethod]
        public void ToHumanReadable_OnSecond_ReturnsSingular() => TimeSpan.FromSeconds(1).ToHumanReadable().Should().Be("1 second");

        [TestMethod]
        public void ToHumanReadable_Minutes_ReturnsMinutes() => TimeSpan.FromMinutes(5).ToHumanReadable().Should().Be("5 minutes");

        [TestMethod]
        public void ToHumanReadable_Hours_ReturnsHours() => TimeSpan.FromHours(2).ToHumanReadable().Should().Be("2 hours");

        [TestMethod]
        public void ToHumanReadable_Days_ReturnsDays() => TimeSpan.FromDays(3).ToHumanReadable().Should().Be("3 days");

        [TestMethod]
        public void ToHumanReadable_Complex_ReturnsMultipleParts()
        {
            var ts = new TimeSpan(2, 5, 30, 0);
            string result = ts.ToHumanReadable();
            result.Should().Contain("2 days");
            result.Should().Contain("5 hours");
            result.Should().Contain("30 minutes");
        }

        [TestMethod]
        public void ToHumanReadable_Negative_PrependsMinus() => TimeSpan.FromHours(-3).ToHumanReadable().Should().StartWith("-");

        [TestMethod]
        public void ToHumanReadable_Years_ReturnsYears() => TimeSpan.FromDays(400).ToHumanReadable().Should().Contain("1 year");
    }
}

