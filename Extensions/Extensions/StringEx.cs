using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNet.Library.Extensions
{
    public static class StringEx
    {
        public static byte[] ToByteArray(this string value)
        {
            return [.. value.ToCharArray().Select(item => (byte)item)];
        }

        public static string FromByteArray(this byte[] data)
        {
            if (data.IsNullOrEmpty())
            {
                return string.Empty;
            }

            StringBuilder sb = new(data.Length);
            sb.Append([.. data.Select(item => (char)item)]);

            return sb.ToString();
        }

        public static string RemoveSpecialCharacters(this string input)
        {
            return new Regex("[^A-Za-z0-9]", RegexOptions.Compiled).Replace(input, string.Empty);
        }

        public static string[] SplitIgnoreCase(this string value, string separator)
        {
            return value.Split(separator, ignoreCase: true);
        }

        public static string[] Split(this string value, string separator, bool ignoreCase = false)
        {
            return ignoreCase
                ? Regex.Split(value, separator, RegexOptions.IgnoreCase)
                : Regex.Split(value, separator);
        }

        public static string FillWith(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static bool NotEquals(this string value, string toCompare)
        {
            return !value.Equals(toCompare);
        }

        public static bool NotEqualsIgnoreCase(this string value, string toCompare)
        {
            return !value.EqualsIgnoreCase(toCompare);
        }

        /// <summary>
        /// Returns true if two strings are equal using <see cref="StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        /// <param name="value">Source string.</param>
        /// <param name="toCompare">Target string.</param>
        /// <returns>True if source and target strings are equal using <see cref="StringComparison.OrdinalIgnoreCase"/>.</returns>
        public static bool EqualsIgnoreCase(this string value, string toCompare)
        {
            return value.Equals(toCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EndsWithIgnoreCase(this string value, string toCompare)
        {
            return value.EndsWith(toCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool StartsWithIgnoreCase(this string value, string toCompare)
        {
            return value.StartsWith(toCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string value, string toCompare)
        {
            return value.Contains(toCompare, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string SafeTrim(this string value)
        {
            if (value.IsNullOrEmpty()) { return string.Empty; }
            return value.Trim();
        }

        public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct
        {
            // default(TEnum) is the zero value
            if (value.IsNullOrEmpty()) { return default; }
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : default;
        }

        public static string UniqueString
        {
            get
            {
                Guid g = Guid.NewGuid();

                string guid = Convert.ToBase64String(g.ToByteArray());
                return guid
                    .Replace("=", "")
                    .Replace("+", "")
                    .Replace("/", "");
            }
        }

        public static bool IsWellFormedEmailAddress(this string source)
        {
            if (source.IsNullOrEmpty())
            {
                return false;
            }

            try
            {
                //return Regex.IsMatch(source,
                //     @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                //     @"(? (\[)(\[(\d{1, 3}\.){3}\d{1,3}\])|(([0 - 9a - z][-\w]*[0 - 9a - z]*\.)+[a-z0-9] [\-a-z0-9]{0,22}[a-z0-9]))$",
                //     RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                return new EmailAddressAttribute().IsValid(source);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if two strings are equal after safe trim using <see cref="StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        /// <param name="value">Source string.</param>
        /// <param name="toCompare">Target string.</param>
        /// <returns>True if source and target strings are equal after safe trim using <see cref="StringComparison.OrdinalIgnoreCase"/>.</returns>
        public static bool EqualsIgnoreCaseWithTrim(this string value, string toCompare)
        {
            return value.SafeTrim().EqualsIgnoreCase(toCompare.SafeTrim());
        }

        /// <summary>
        /// Returns true if two strings are equal after safe trim.
        /// </summary>
        /// <param name="value">Source string.</param>
        /// <param name="toCompare">Target string.</param>
        /// <returns>True if source and target strings are equal after safe trim.</returns>
        public static bool EqualsWithTrim(this string value, string toCompare)
        {
            return value.SafeTrim().Equals(toCompare.SafeTrim());
        }

        /// <summary>
        /// Returns true if string starts with another string after safe trim.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCaseWithTrim(this string value, string toCompare)
        {
            return value.SafeTrim().StartsWithIgnoreCase(toCompare.SafeTrim());
        }
    }
}
