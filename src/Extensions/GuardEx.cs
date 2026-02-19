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
    }
}

