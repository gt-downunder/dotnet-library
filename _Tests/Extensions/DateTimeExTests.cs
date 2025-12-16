using FluentAssertions;
using DotNet.Library.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNet.Library.Tests.Extensions
{
    [TestClass]
    public class DateTimeExTests : BaseExtensionTest
    {
        [TestMethod]
        public void ToFormattedDate_NullDateTime_ReturnsEmptyString()
        {
            var result = ((DateTime?)null).ToFormattedDate();

            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void ToFormattedDate_ValidDateTime_Success()
        {
            DateTime? testDate = DateTime.Now;

            var result = testDate.ToFormattedDate();

            result.Should().Be(testDate.Value.ToString("yyyy-MM-dd"));
        }

        [TestMethod]
        public void ToFormattedDateTime_NullDateTime_ReturnsEmptyString()
        {
            var result = ((DateTime?)null).ToFormattedDateTime();

            result.Should().Be(string.Empty);
        }

        [TestMethod]
        public void ToFormattedDateTime_ValidDateTime_Success()
        {
            DateTime? testDate = DateTime.Now;

            var result = testDate.ToFormattedDateTime();

            result.Should().Be(testDate.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("2023-13-32")]
        [DataRow("2023-03-23T11:51:28")]
        [DataRow("yyyy-MM-dd")]
        public void FromFormattedDate_InvalidFormat_ReturnsNull(string testDate)
        {
            DateTime? result = testDate.FromFormattedDate();

            result.Should().BeNull();
        }

        [TestMethod]
        public void FromFormattedDate_ValidFormat_Success()
        {
            const string testDate = "2023-03-23";

            DateTime? result = testDate.FromFormattedDate();

            result.Should().Be(new DateTime(2023, 3, 23));
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("2023-03-23")]
        [DataRow("2023-13-31T11:51:28")]
        [DataRow("yyyy-MM-ddTHH:mm:ss")]
        public void FromFormattedDateTime_InvalidFormat_ReturnsNull(string testDate)
        {
            DateTime? result = testDate.FromFormattedDateTime();

            result.Should().BeNull();
        }

        [TestMethod]
        public void FromFormattedDateTime_ValidFormat_Success()
        {
            const string testDate = "2023-03-23T11:57:28";

            DateTime? result = testDate.FromFormattedDateTime();

            result.Should().Be(new DateTime(2023, 3, 23, 11, 57, 28));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void AddWeeks_Succeeds(int weeksToAdd)
        {
            var now = DateTime.Now;
            var result = now.AddWeeks(weeksToAdd);

            result.DayOfWeek.Should().Be(now.DayOfWeek);
            result.Should().BeExactly(new TimeSpan(weeksToAdd * 7, 0, 0, 0, 0)).After(now);
        }

        [TestMethod]
        public void TruncateMilliseconds_Succeeds()
        {
            var dt = new DateTime(1999, 12, 31, 0, 0, 0, 999);
            dt.Millisecond.Should().Be(999);

            var result = dt.TruncateMilliseconds();
            result.Millisecond.Should().Be(0);
        }

        [TestMethod]
        [DataRow("2019-09-30")] // Monday
        [DataRow("2019-10-01")] // Tuesday
        [DataRow("2019-10-02")] // Wednesday
        [DataRow("2019-10-03")] // Thursday
        [DataRow("2019-10-04")] // Friday
        public void IsWeekday_MondayToFriday_ReturnsTrue(string dateTime)
        {
            var dt = Convert.ToDateTime(dateTime);
            dt.IsWeekday().Should().BeTrue();
        }

        [TestMethod]
        [DataRow("2019-10-05")] // Saturday
        [DataRow("2019-10-06")] // Sunday
        public void IsWeekday_SaturdayAndSunday_ReturnsFalse(string dateTime)
        {
            var dt = Convert.ToDateTime(dateTime);
            dt.IsWeekday().Should().BeFalse();
        }

        [TestMethod]
        [DataRow("2019-10-05")] // Saturday
        [DataRow("2019-10-06")] // Sunday
        public void IsWeekend_SaturdayAndSunday_ReturnsTrue(string dateTime)
        {
            var dt = Convert.ToDateTime(dateTime);
            dt.IsWeekend().Should().BeTrue();
        }

        [TestMethod]
        [DataRow("2019-09-30")] // Monday
        [DataRow("2019-10-01")] // Tuesday
        [DataRow("2019-10-02")] // Wednesday
        [DataRow("2019-10-03")] // Thursday
        [DataRow("2019-10-04")] // Friday
        public void IsWeekend_MondayToFriday_ReturnsFalse(string dateTime)
        {
            var dt = Convert.ToDateTime(dateTime);
            dt.IsWeekend().Should().BeFalse();
        }
    }
}
