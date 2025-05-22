using System;
using System.Globalization;

namespace DotNet.Library.Extensions
{
    public static class DateTimeEx
    {
        public static string ConstantsDate => "yyyy-MM-dd";
        public static string ConstantsDateTime => "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Formats the given nullable <see cref="DateTime" /> to a <see cref="string"/>
        /// </summary>
        /// <returns>An empty string if the nullable <see cref="DateTime?"/> is null, otherwise a string using the format "yyyy-MM-dd"</returns>
        public static string ToFormattedDate(this DateTime? date)
        {
            if (!date.HasValue) { return string.Empty; }
            return date.Value.ToFormattedDate();
        }

        /// <summary>
        /// Formats the given <see cref="DateTime" /> to a <see cref="string"/>
        /// </summary>
        /// <returns>A string representation of the provided <see cref="DateTime"/> using the format "yyyy-MM-dd"</returns>
        public static string ToFormattedDate(this DateTime date)
        {
            return date.ToString(ConstantsDate, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the given nullable <see cref="DateTime" /> to a <see cref="string"/>
        /// </summary>
        /// <returns>An empty string if the nullable <see cref="DateTime?"/> is null, otherwise a string using the format "yyyy-MM-ddTHH:mm:ss"</returns>
        public static string ToFormattedDateTime(this DateTime? date)
        {
            if (!date.HasValue) { return string.Empty; }
            return date.Value.ToFormattedDateTime();
        }

        /// <summary>
        /// Formats the given <see cref="DateTime" /> to a <see cref="string"/>
        /// </summary>
        /// <returns>A string representation of the provided <see cref="DateTime"/> using the format "yyyy-MM-ddTHH:mm:ss"</returns>
        public static string ToFormattedDateTime(this DateTime date)
        {
            return date.ToString(ConstantsDateTime, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Attempts to convert the given <see cref="string"/> to a <see cref="DateTime" />
        /// </summary>
        /// <param name="date">A string, representing a valid <see cref="DateTime"/>, which must be formatted exactly as "yyyy-MM-dd"</param>
        /// <returns>A null nullable <see cref="DateTime"/> if the conversion fails, otherwise a <see cref="DateTime"/></returns>
        public static DateTime? FromFormattedDate(this string date)
        {
            if (DateTime.TryParseExact(date, ConstantsDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Attempts to convert the given <see cref="string"/> to a <see cref="DateTime" />
        /// </summary>
        /// <param name="date">A string, representing a valid <see cref="DateTime"/>, which must be formatted exactly as "yyyy-MM-ddTHH:mm:ss"</param>
        /// <returns>A null nullable <see cref="DateTime"/> if the conversion fails, otherwise a <see cref="DateTime"/></returns>
        public static DateTime? FromFormattedDateTime(this string date)
        {
            if (DateTime.TryParseExact(date, ConstantsDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Add the specified number of <paramref name="weeks"/> to the given <paramref name="date"/>
        /// </summary>
        /// <returns>A <see cref="DateTime"/> object offset by the number of weeks specified</returns>
        public static DateTime AddWeeks(this DateTime date, int weeks)
        {
            int daysToAdd = weeks * 7;
            return date.AddDays(daysToAdd);
        }

        /// <summary>
        /// Returns a new DateTime with milliseconds zeroed out. Useful for unit testing
        /// as Entity Framework Core In-Memory Provider does not store datetimes with that much precision.
        /// </summary>
        public static DateTime TruncateMilliseconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        /// <summary>
        /// Determines if the given <see cref="DateTime"/> object falls on a weekday (Monday - Friday)
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/>object to check</param>
        /// <returns>True if it is a weekday, otherwise false</returns>
        public static bool IsWeekday(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// Determines if the given <see cref="DateTime"/> object falls on a weekend (Saturday or Sunday)
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/>object to check</param>
        /// <returns>True if it is a weekend, otherwise false</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
