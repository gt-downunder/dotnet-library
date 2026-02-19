using System.Globalization;
using System.Text;
using System.Text.Json;
using Grondo.Utilities;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="object"/> and general-purpose conversions.
    /// </summary>
    public static class ObjectEx
    {
        extension(object? obj)
        {
            /// <summary>
            /// Determines whether the specified object represents a numeric value.
            /// Supports numeric types and numeric strings.
            /// </summary>
            /// <returns><c>true</c> if the object is numeric; otherwise, <c>false</c>.</returns>
            public bool IsNumeric()
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
            /// <returns>
            /// A <see cref="double"/> value if conversion succeeds; otherwise, <c>null</c>.
            /// </returns>
            public double? ToNullableDouble() =>
                obj is null ? null :
                double.TryParse(obj.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double value) ? value :
                null;

            /// <summary>
            /// Attempts to convert the specified object to a nullable <see cref="int"/>.
            /// </summary>
            /// <returns>
            /// An <see cref="int"/> value if conversion succeeds; otherwise, <c>null</c>.
            /// </returns>
            public int? ToNullableInteger() =>
                obj is null ? null :
                int.TryParse(obj.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out int value) ? value : null;

            /// <summary>
            /// Attempts to convert the specified object to a nullable <see cref="bool"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="bool"/> value if conversion succeeds; otherwise, <c>null</c>.
            /// </returns>
            public bool? ToNullableBoolean() =>
                obj is null ? null :
                bool.TryParse(obj.ToString(), out bool value) ? value : null;

            /// <summary>
            /// Attempts to convert the specified object to a nullable <see cref="DateTime"/>.
            /// </summary>
            /// <param name="format">
            /// An optional format string. If provided, parsing is performed using
            /// <see cref="DateTime.ParseExact(string, string, IFormatProvider?)"/>.
            /// If omitted, parsing uses <see cref="DateTime.Parse(string, IFormatProvider?)"/>.
            /// </param>
            /// <returns>
            /// A <see cref="DateTime"/> value if conversion succeeds; otherwise, <c>null</c>.
            /// </returns>
            public DateTime? ToNullableDateTime(string format = "")
            {
                if (obj is null) return null;
                string? s = obj.ToString();
                if (string.IsNullOrWhiteSpace(s)) return null;

                return string.IsNullOrEmpty(format)
                    ? DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out global::System.DateTime dt1) ? dt1 : null
                    : DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out global::System.DateTime dt2)
                        ? dt2
                        : null;
            }
        }

        extension<T>(T obj)
        {
            /// <summary>
            /// Converts an object to an <see cref="StringContent"/> instance suitable for HTTP requests.
            /// If the object is a string, it is used directly; otherwise, the object is serialized to JSON.
            /// </summary>
            /// <param name="contentType">The MIME content type (e.g., "application/json").</param>
            /// <returns>A <see cref="StringContent"/> containing the serialized object.</returns>
            public StringContent ToStringContent(string contentType) =>
                obj is string s
                    ? new StringContent(s, Encoding.UTF8, contentType)
                    : new StringContent(JsonSerializer.Serialize(obj, JsonDefaults.Default), Encoding.UTF8, contentType);
        }
    }
}

