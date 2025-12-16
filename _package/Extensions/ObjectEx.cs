using System.Globalization;
using System.Text;
using System.Text.Json;

namespace DotNet.Library.Extensions
{
    public static class ObjectEx
    {
        /// <summary>
        /// Determines whether the specified object represents a numeric value.
        /// Supports numeric types and numeric strings.
        /// </summary>
        /// <param name="obj">The object to evaluate.</param>
        /// <returns><c>true</c> if the object is numeric; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this object? obj)
        {
            if (obj is null) return false;

            return obj switch
            {
                string s => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out _),
                IConvertible c => c.GetTypeCode() switch
                {
                    TypeCode.Byte or TypeCode.SByte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64
                        or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64
                        or TypeCode.Decimal or TypeCode.Double or TypeCode.Single => true,
                    _ => false
                },
                _ => false
            };
        }

        /// <summary>
        /// Attempts to convert the specified object to a nullable <see cref="double"/>.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>
        /// A <see cref="double"/> value if conversion succeeds; otherwise, <c>null</c>.
        /// </returns>
        public static double? ToNullableDouble(this object? obj) =>
            obj is null ? null :
            double.TryParse(obj.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value :
            null;

        /// <summary>
        /// Attempts to convert the specified object to a nullable <see cref="int"/>.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>
        /// An <see cref="int"/> value if conversion succeeds; otherwise, <c>null</c>.
        /// </returns>
        public static int? ToNullableInteger(this object? obj) =>
            obj is null ? null :
            int.TryParse(obj.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : null;

        /// <summary>
        /// Attempts to convert the specified object to a nullable <see cref="bool"/>.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <returns>
        /// A <see cref="bool"/> value if conversion succeeds; otherwise, <c>null</c>.
        /// </returns>
        public static bool? ToNullableBoolean(this object? obj) =>
            obj is null ? null :
            bool.TryParse(obj.ToString(), out var value) ? value : null;

        /// <summary>
        /// Attempts to convert the specified object to a nullable <see cref="DateTime"/>.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <param name="format">
        /// An optional format string. If provided, parsing is performed using <see cref="DateTime.ParseExact"/>.
        /// If omitted, parsing uses <see cref="DateTime.Parse"/>.
        /// </param>
        /// <returns>
        /// A <see cref="DateTime"/> value if conversion succeeds; otherwise, <c>null</c>.
        /// </returns>
        public static DateTime? ToNullableDateTime(this object? obj, string format = "")
        {
            if (obj is null) return null;
            var s = obj.ToString();
            if (string.IsNullOrWhiteSpace(s)) return null;

            return string.IsNullOrEmpty(format)
                ? DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt1) ? dt1 : null
                : DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt2)
                    ? dt2
                    : null;
        }

        /// <summary>
        /// Determines whether the specified string is <c>null</c>, empty, or consists only of whitespace characters.
        /// </summary>
        /// <param name="value">The string to evaluate.</param>
        /// <returns><c>true</c> if the string is <c>null</c>, empty, or whitespace; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty(this string? value) =>
            string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Determines whether the specified sequence is <c>null</c> or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to evaluate.</param>
        /// <returns><c>true</c> if the sequence is <c>null</c> or empty; otherwise, <c>false</c>.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) =>
            source == null || !source.Any();

        /// <summary>
        /// Determines whether the specified sequence is not <c>null</c> and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="source">The sequence to evaluate.</param>
        /// <returns><c>true</c> if the sequence is not <c>null</c> and contains elements; otherwise, <c>false</c>.</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? source) =>
            source != null && source.Any();

        /// <summary>
        /// Converts an object to an <see cref="StringContent"/> instance suitable for HTTP requests.
        /// If the object is a string, it is used directly; otherwise, the object is serialized to JSON.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <param name="contentType">The MIME content type (e.g., "application/json").</param>
        /// <returns>A <see cref="StringContent"/> containing the serialized object.</returns>
        public static StringContent ToStringContent<T>(this T obj, string contentType) =>
            obj is string s
                ? new StringContent(s, Encoding.UTF8, contentType)
                : new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, contentType);
    }
}