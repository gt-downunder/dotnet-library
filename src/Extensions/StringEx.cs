using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="string"/>.
    /// </summary>
    public static partial class StringEx
    {
        [GeneratedRegex("[^A-Za-z0-9]")]
        private static partial Regex NonAlphaNumericRegex();

        [GeneratedRegex("[^a-z0-9]+")]
        private static partial Regex SlugSeparatorRegex();

        [GeneratedRegex("(^-|-$)")]
        private static partial Regex SlugTrimRegex();

        private static readonly EmailAddressAttribute _emailAddressValidator = new();

        extension(string value)
        {
            /// <summary>
            /// Converts the string to a UTF-8 encoded byte array.
            /// </summary>
            /// <returns>A UTF-8 encoded byte array representation of the string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            public byte[] ToByteArray()
            {
                ArgumentNullException.ThrowIfNull(value);
                return Encoding.UTF8.GetBytes(value);
            }

            /// <summary>
            /// Removes all non-alphanumeric characters from the string.
            /// </summary>
            /// <returns>The string with only letters and digits.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            public string RemoveSpecialCharacters()
            {
                ArgumentNullException.ThrowIfNull(value);
                return NonAlphaNumericRegex().Replace(value, string.Empty);
            }

            /// <summary>
            /// Splits the string into substrings based on a regex pattern.
            /// </summary>
            /// <param name="pattern">The regex pattern to split on.</param>
            /// <param name="ignoreCase">Whether to ignore case in the regex pattern.</param>
            /// <returns>An array of substrings.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="pattern"/> is null.</exception>
            public string[] RegexSplit(string pattern, bool ignoreCase = false)
            {
                ArgumentNullException.ThrowIfNull(value);
                ArgumentNullException.ThrowIfNull(pattern);
                return Regex.Split(value, pattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
            }

            /// <summary>
            /// Determines whether two strings are equal using case-insensitive comparison.
            /// </summary>
            public bool EqualsIgnoreCase(string toCompare) =>
                string.Equals(value, toCompare, StringComparison.OrdinalIgnoreCase);

            /// <summary>
            /// Determines whether the string ends with the specified substring using case-insensitive comparison.
            /// </summary>
            public bool EndsWithIgnoreCase(string toCompare) =>
                value?.EndsWith(toCompare, StringComparison.OrdinalIgnoreCase) ?? false;

            /// <summary>
            /// Determines whether the string starts with the specified substring using case-insensitive comparison.
            /// </summary>
            public bool StartsWithIgnoreCase(string toCompare) =>
                value?.StartsWith(toCompare, StringComparison.OrdinalIgnoreCase) ?? false;

            /// <summary>
            /// Determines whether the string contains the specified substring using case-insensitive comparison.
            /// </summary>
            public bool ContainsIgnoreCase(string toCompare) =>
                value is not null && toCompare is not null && value.Contains(toCompare, StringComparison.OrdinalIgnoreCase);

            /// <summary>
            /// Trims the string safely, returning an empty string if the input is null or whitespace.
            /// </summary>
            public string SafeTrim() =>
                string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

            /// <summary>
            /// Attempts to parse the string into an enum of type <typeparamref name="TEnum"/>.
            /// Returns the default value if parsing fails.
            /// </summary>
            public TEnum ToEnum<TEnum>(bool ignoreCase = true) where TEnum : struct =>
                Enum.TryParse(value, ignoreCase, out TEnum result) ? result : default;

            /// <summary>
            /// Determines whether the string is a well-formed email address.
            /// </summary>
            /// <returns><c>true</c> if the string is a valid email address; otherwise, <c>false</c>.</returns>
            public bool IsWellFormedEmailAddress() =>
                !string.IsNullOrWhiteSpace(value) && _emailAddressValidator.IsValid(value);

            /// <summary>
            /// Determines whether two strings are equal using case-insensitive comparison after trimming whitespace.
            /// </summary>
            public bool EqualsIgnoreCaseWithTrim(string toCompare) =>
                value.SafeTrim().EqualsIgnoreCase(toCompare.SafeTrim());

            /// <summary>
            /// Determines whether two strings are equal after trimming whitespace.
            /// </summary>
            public bool EqualsWithTrim(string toCompare) =>
                value.SafeTrim().Equals(toCompare.SafeTrim(), StringComparison.Ordinal);

            /// <summary>
            /// Determines whether the string starts with the specified substring using case-insensitive comparison after trimming whitespace.
            /// </summary>
            public bool StartsWithIgnoreCaseWithTrim(string toCompare) =>
                value.SafeTrim().StartsWithIgnoreCase(toCompare.SafeTrim());

            /// <summary>
            /// Truncates the string to the specified maximum length, appending a suffix if truncation occurs.
            /// </summary>
            /// <param name="maxLength">The maximum length of the returned string, including the suffix.</param>
            /// <param name="suffix">The suffix to append when truncation occurs. Defaults to "...".</param>
            /// <returns>The original string if shorter than <paramref name="maxLength"/>, or the truncated string with suffix.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is negative.</exception>
            public string Truncate(int maxLength, string suffix = "...")
            {
                ArgumentOutOfRangeException.ThrowIfNegative(maxLength);

                if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                    return value ?? string.Empty;

                int suffixLength = suffix?.Length ?? 0;
                if (maxLength <= suffixLength)
                    return (suffix ?? string.Empty)[..maxLength];

                return string.Concat(value.AsSpan(0, maxLength - suffixLength), suffix);
            }

            /// <summary>
            /// Encodes the string to a Base64 representation using UTF-8 encoding.
            /// </summary>
            /// <returns>The Base64-encoded string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            public string ToBase64()
            {
                ArgumentNullException.ThrowIfNull(value);
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            }

            /// <summary>
            /// Decodes a Base64-encoded string back to its original UTF-8 representation.
            /// </summary>
            /// <returns>The decoded string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            /// <exception cref="FormatException">Thrown if the string is not a valid Base64 string.</exception>
            public string FromBase64()
            {
                ArgumentNullException.ThrowIfNull(value);
                return Encoding.UTF8.GetString(Convert.FromBase64String(value));
            }

            /// <summary>
            /// Masks the string, revealing only the last specified number of characters.
            /// The remaining characters are replaced with the mask character.
            /// </summary>
            /// <param name="visibleChars">The number of trailing characters to leave visible. Defaults to 4.</param>
            /// <param name="maskChar">The character to use for masking. Defaults to '*'.</param>
            /// <returns>The masked string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            public string Mask(int visibleChars = 4, char maskChar = '*')
            {
                ArgumentNullException.ThrowIfNull(value);

                if (value.Length <= visibleChars)
                    return value;

                int maskLength = value.Length - visibleChars;
                return new string(maskChar, maskLength) + value[maskLength..];
            }

            /// <summary>
            /// Converts the string to a URL-friendly slug by lowercasing, replacing non-alphanumeric
            /// characters with hyphens, and trimming leading/trailing hyphens.
            /// </summary>
            /// <returns>A URL-friendly slug representation of the string.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the string is null.</exception>
            public string ToSlug()
            {
                ArgumentNullException.ThrowIfNull(value);

                string slug = value.ToLowerInvariant();
                slug = SlugSeparatorRegex().Replace(slug, "-");
                slug = SlugTrimRegex().Replace(slug, string.Empty);
                return slug;
            }
        }

        extension(string? value)
        {
            /// <summary>
            /// Determines whether the string is <c>null</c>, empty, or consists only of whitespace characters.
            /// Equivalent to <see cref="string.IsNullOrWhiteSpace(string?)"/> as an extension method.
            /// </summary>
            /// <returns><c>true</c> if the string is <c>null</c>, empty, or whitespace; otherwise, <c>false</c>.</returns>
            public bool IsNullOrWhiteSpace() =>
                string.IsNullOrWhiteSpace(value);
        }

        extension(byte[]? data)
        {
            /// <summary>
            /// Converts a UTF-8 encoded byte array back into a string.
            /// </summary>
            /// <returns>The decoded string, or an empty string if the byte array is null or empty.</returns>
            public string FromByteArray() =>
                data == null || data.Length == 0 ? string.Empty : Encoding.UTF8.GetString(data);
        }
    }
}
