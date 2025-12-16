using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Library.Extensions
{
    public static class StringEx
    {
        /// <summary>
        /// Converts the string to a UTF-8 encoded byte array.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <returns>A UTF-8 encoded byte array representation of the string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        public static byte[] ToByteArray(this string value) =>
            Encoding.UTF8.GetBytes(value ?? throw new ArgumentNullException(nameof(value)));

        /// <summary>
        /// Converts a UTF-8 encoded byte array back into a string.
        /// </summary>
        /// <param name="data">The byte array to convert.</param>
        /// <returns>The decoded string, or an empty string if <paramref name="data"/> is null or empty.</returns>
        public static string FromByteArray(this byte[]? data) =>
            data == null || data.Length == 0 ? string.Empty : Encoding.UTF8.GetString(data);

        private static readonly Regex NonAlphaNumeric = new("[^A-Za-z0-9]", RegexOptions.Compiled);

        /// <summary>
        /// Removes all non-alphanumeric characters from the string.
        /// </summary>
        /// <param name="input">The source string.</param>
        /// <returns>The string with only letters and digits.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is null.</exception>
        public static string RemoveSpecialCharacters(this string input) =>
            NonAlphaNumeric.Replace(input ?? throw new ArgumentNullException(nameof(input)), string.Empty);

        /// <summary>
        /// Splits the string into substrings based on a regex pattern.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="pattern">The regex pattern to split on.</param>
        /// <param name="ignoreCase">Whether to ignore case in the regex pattern.</param>
        /// <returns>An array of substrings.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> or <paramref name="pattern"/> is null.</exception>
        public static string[] Split(this string value, string pattern, bool ignoreCase = false) =>
            Regex.Split(value ?? throw new ArgumentNullException(nameof(value)),
                pattern ?? throw new ArgumentNullException(nameof(pattern)),
                ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

        /// <summary>
        /// Determines whether two strings are equal using case-insensitive comparison.
        /// </summary>
        public static bool EqualsIgnoreCase(this string value, string toCompare) =>
            string.Equals(value, toCompare, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Determines whether the string ends with the specified substring using case-insensitive comparison.
        /// </summary>
        public static bool EndsWithIgnoreCase(this string value, string toCompare) =>
            value?.EndsWith(toCompare, StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Determines whether the string starts with the specified substring using case-insensitive comparison.
        /// </summary>
        public static bool StartsWithIgnoreCase(this string value, string toCompare) =>
            value?.StartsWith(toCompare, StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Determines whether the string contains the specified substring using case-insensitive comparison.
        /// </summary>
        public static bool ContainsIgnoreCase(this string value, string toCompare) =>
            value?.IndexOf(toCompare, StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// Trims the string safely, returning an empty string if the input is null or whitespace.
        /// </summary>
        public static string SafeTrim(this string value) =>
            string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

        /// <summary>
        /// Attempts to parse the string into an enum of type <typeparamref name="TEnum"/>.
        /// Returns the default value if parsing fails.
        /// </summary>
        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct =>
            Enum.TryParse(value, ignoreCase, out TEnum result) ? result : default;

        /// <summary>
        /// Generates a unique string identifier using a GUID in "N" format (32 digits).
        /// </summary>
        public static string UniqueString => Guid.NewGuid().ToString("N");

        /// <summary>
        /// Determines whether the string is a well-formed email address.
        /// </summary>
        public static bool IsWellFormedEmailAddress(this string source) =>
            !string.IsNullOrWhiteSpace(source) && new EmailAddressAttribute().IsValid(source);

        /// <summary>
        /// Determines whether two strings are equal using case-insensitive comparison after trimming whitespace.
        /// </summary>
        public static bool EqualsIgnoreCaseWithTrim(this string value, string toCompare) =>
            value.SafeTrim().EqualsIgnoreCase(toCompare.SafeTrim());

        /// <summary>
        /// Determines whether two strings are equal after trimming whitespace.
        /// </summary>
        public static bool EqualsWithTrim(this string value, string toCompare) =>
            value.SafeTrim().Equals(toCompare.SafeTrim());

        /// <summary>
        /// Determines whether the string starts with the specified substring using case-insensitive comparison after trimming whitespace.
        /// </summary>
        public static bool StartsWithIgnoreCaseWithTrim(this string value, string toCompare) =>
            value.SafeTrim().StartsWithIgnoreCase(toCompare.SafeTrim());
    }
}