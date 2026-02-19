using System.Numerics;
using System.Text.RegularExpressions;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides fluent guard clause extension methods for argument validation.
    /// </summary>
    public static class GuardEx
    {
        extension<T>(T? value) where T : class
        {
            /// <summary>
            /// Throws <see cref="ArgumentNullException"/> if the value is null.
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The non-null value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
            public T ThrowIfNull(string? paramName = null) =>
                value ?? throw new ArgumentNullException(paramName);
        }

        extension(string? value)
        {
            /// <summary>
            /// Throws <see cref="ArgumentException"/> if the string is null, empty, or whitespace.
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The non-null, non-empty string value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
            /// <exception cref="ArgumentException">Thrown if the value is empty or whitespace.</exception>
            public string ThrowIfNullOrWhiteSpace(string? paramName = null) =>
                string.IsNullOrWhiteSpace(value)
                    ? throw (value is null
                        ? new ArgumentNullException(paramName)
                        : new ArgumentException("Value cannot be empty or whitespace.", paramName))
                    : value;

            /// <summary>
            /// Throws <see cref="ArgumentException"/> if the string does not match the specified regex pattern.
            /// </summary>
            /// <param name="regexPattern">The regular expression pattern to validate against.</param>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The validated string value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the value or pattern is null.</exception>
            /// <exception cref="ArgumentException">Thrown if the value does not match the pattern.</exception>
            public string ThrowIfInvalidFormat(string regexPattern, string? paramName = null)
            {
                ArgumentNullException.ThrowIfNull(value, paramName);
                ArgumentNullException.ThrowIfNull(regexPattern);
                if (!Regex.IsMatch(value, regexPattern))
                    throw new ArgumentException($"Value does not match the required format '{regexPattern}'.", paramName);
                return value;
            }
        }

        extension<T>(T value) where T : struct
        {
            /// <summary>
            /// Throws <see cref="ArgumentException"/> if the value equals its type's default.
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The non-default value.</returns>
            /// <exception cref="ArgumentException">Thrown if the value is the default value.</exception>
            public T ThrowIfDefault(string? paramName = null) =>
                EqualityComparer<T>.Default.Equals(value, default)
                    ? throw new ArgumentException("Value cannot be the default value.", paramName)
                    : value;
        }

        extension<T>(IEnumerable<T>? value)
        {
            /// <summary>
            /// Throws <see cref="ArgumentException"/> if the collection is null or empty.
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The non-null, non-empty collection.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
            /// <exception cref="ArgumentException">Thrown if the value is empty.</exception>
            public IEnumerable<T> ThrowIfEmpty(string? paramName = null)
            {
                ArgumentNullException.ThrowIfNull(value, paramName);
                if (!value.Any()) throw new ArgumentException("Collection cannot be empty.", paramName);
                return value;
            }
        }

        extension<T>(T value) where T : INumber<T>
        {
            /// <summary>
            /// Throws <see cref="ArgumentOutOfRangeException"/> if the value is negative (less than zero).
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The validated non-negative value.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is negative.</exception>
            public T ThrowIfNegative(string? paramName = null) =>
                value < T.Zero
                    ? throw new ArgumentOutOfRangeException(paramName, value, "Value must not be negative.")
                    : value;

            /// <summary>
            /// Throws <see cref="ArgumentOutOfRangeException"/> if the value is negative or zero.
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The validated positive value.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is negative or zero.</exception>
            public T ThrowIfNegativeOrZero(string? paramName = null) =>
                value <= T.Zero
                    ? throw new ArgumentOutOfRangeException(paramName, value, "Value must be positive.")
                    : value;

            /// <summary>
            /// Throws <see cref="ArgumentOutOfRangeException"/> if the value is zero.
            /// </summary>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The validated non-zero value.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is zero.</exception>
            public T ThrowIfZero(string? paramName = null) =>
                value == T.Zero
                    ? throw new ArgumentOutOfRangeException(paramName, value, "Value must not be zero.")
                    : value;

            /// <summary>
            /// Throws <see cref="ArgumentOutOfRangeException"/> if the value is outside the specified range [min, max].
            /// </summary>
            /// <param name="min">The minimum allowed value (inclusive).</param>
            /// <param name="max">The maximum allowed value (inclusive).</param>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The validated value within range.</returns>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is outside the range.</exception>
            public T ThrowIfOutOfRange(T min, T max, string? paramName = null) =>
                value < min || value > max
                    ? throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max}.")
                    : value;
        }

        extension<T>(T value)
        {
            /// <summary>
            /// Throws <see cref="ArgumentException"/> if the predicate returns <c>true</c> for the value.
            /// </summary>
            /// <param name="predicate">The condition that, when true, causes an exception to be thrown.</param>
            /// <param name="message">The error message for the exception.</param>
            /// <param name="paramName">The name of the parameter.</param>
            /// <returns>The validated value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="predicate"/> is null.</exception>
            /// <exception cref="ArgumentException">Thrown if the predicate returns true.</exception>
            public T ThrowIf(Func<T, bool> predicate, string message, string? paramName = null)
            {
                ArgumentNullException.ThrowIfNull(predicate);
                if (predicate(value))
                    throw new ArgumentException(message, paramName);
                return value;
            }
        }
    }
}

