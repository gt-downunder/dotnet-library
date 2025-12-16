using System.Globalization;

namespace DotNet.Library.Extensions
{
    public static class DateTimeEx
    {
        /// <summary>
        /// Standard date format string: "yyyy-MM-dd".
        /// </summary>
        public const string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// Standard date-time format string: "yyyy-MM-ddTHH:mm:ss".
        /// </summary>
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Formats the specified <see cref="DateTime"/> as a string using the standard date format ("yyyy-MM-dd").
        /// </summary>
        /// <param name="date">The date to format.</param>
        /// <returns>A string representation of the date.</returns>
        public static string ToFormattedDate(this DateTime date) =>
            date.ToString(DateFormat, CultureInfo.InvariantCulture);

        /// <summary>
        /// Formats the specified nullable <see cref="DateTime"/> as a string using the standard date format ("yyyy-MM-dd").
        /// Returns an empty string if the value is <c>null</c>.
        /// </summary>
        /// <param name="date">The nullable date to format.</param>
        /// <returns>A string representation of the date, or an empty string if <paramref name="date"/> is <c>null</c>.</returns>
        public static string ToFormattedDate(this DateTime? date) =>
            date.HasValue ? date.Value.ToFormattedDate() : string.Empty;

        /// <summary>
        /// Formats the specified <see cref="DateTime"/> as a string using the standard date-time format ("yyyy-MM-ddTHH:mm:ss").
        /// </summary>
        /// <param name="date">The date-time to format.</param>
        /// <returns>A string representation of the date-time.</returns>
        public static string ToFormattedDateTime(this DateTime date) =>
            date.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

        /// <summary>
        /// Formats the specified nullable <see cref="DateTime"/> as a string using the standard date-time format ("yyyy-MM-ddTHH:mm:ss").
        /// Returns an empty string if the value is <c>null</c>.
        /// </summary>
        /// <param name="date">The nullable date-time to format.</param>
        /// <returns>A string representation of the date-time, or an empty string if <paramref name="date"/> is <c>null</c>.</returns>
        public static string ToFormattedDateTime(this DateTime? date) =>
            date.HasValue ? date.Value.ToFormattedDateTime() : string.Empty;

        /// <summary>
        /// Attempts to parse a string formatted as "yyyy-MM-dd" into a <see cref="DateTime"/>.
        /// Returns <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="date">The string to parse.</param>
        /// <returns>A <see cref="DateTime"/> if parsing succeeds; otherwise, <c>null</c>.</returns>
        public static DateTime? FromFormattedDate(this string date) =>
            DateTime.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)
                ? result
                : null;

        /// <summary>
        /// Attempts to parse a string formatted as "yyyy-MM-ddTHH:mm:ss" into a <see cref="DateTime"/>.
        /// Returns <c>null</c> if parsing fails.
        /// </summary>
        /// <param name="date">The string to parse.</param>
        /// <returns>A <see cref="DateTime"/> if parsing succeeds; otherwise, <c>null</c>.</returns>
        public static DateTime? FromFormattedDateTime(this string date) =>
            DateTime.TryParseExact(date, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var result)
                ? result
                : null;

        /// <summary>
        /// Attempts to parse a string formatted as "yyyy-MM-dd" into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The string to parse.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="DateTime"/> if parsing succeeded.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryFromFormattedDate(this string date, out DateTime result) =>
            DateTime.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

        /// <summary>
        /// Attempts to parse a string formatted as "yyyy-MM-ddTHH:mm:ss" into a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The string to parse.</param>
        /// <param name="result">When this method returns, contains the parsed <see cref="DateTime"/> if parsing succeeded.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise, <c>false</c>.</returns>
        public static bool TryFromFormattedDateTime(this string date, out DateTime result) =>
            DateTime.TryParseExact(date, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);

        /// <summary>
        /// Adds the specified number of weeks to the given <see cref="DateTime"/>.
        /// </summary>
        /// <param name="date">The date to add weeks to.</param>
        /// <param name="weeks">The number of weeks to add.</param>
        /// <returns>A new <see cref="DateTime"/> offset by the specified number of weeks.</returns>
        public static DateTime AddWeeks(this DateTime date, int weeks) =>
            date.AddDays(weeks * 7);

        /// <summary>
        /// Returns a new <see cref="DateTime"/> with milliseconds truncated.
        /// Useful for scenarios where precision beyond seconds is not required.
        /// </summary>
        /// <param name="date">The date-time to truncate.</param>
        /// <returns>A new <see cref="DateTime"/> with milliseconds set to zero.</returns>
        public static DateTime TruncateMilliseconds(this DateTime date) =>
            new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> falls on a weekday (Monday through Friday).
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns><c>true</c> if the date is a weekday; otherwise, <c>false</c>.</returns>
        public static bool IsWeekday(this DateTime date) =>
            date.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Friday;

        /// <summary>
        /// Determines whether the specified <see cref="DateTime"/> falls on a weekend (Saturday or Sunday).
        /// </summary>
        /// <param name="date">The date to evaluate.</param>
        /// <returns><c>true</c> if the date is a weekend; otherwise, <c>false</c>.</returns>
        public static bool IsWeekend(this DateTime date) =>
            date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }
}