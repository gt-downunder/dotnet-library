namespace Grondo.Utilities
{
    /// <summary>
    /// Provides factory methods for generating string values.
    /// </summary>
    public static class StringFactory
    {
        private const string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const string AllChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";

        /// <summary>
        /// Generates a unique string identifier using a GUID in "N" format (32 hex digits, no hyphens).
        /// </summary>
        public static string UniqueString => Guid.NewGuid().ToString("N");

        /// <summary>
        /// Creates a random string of the specified length.
        /// This method is thread-safe.
        /// </summary>
        /// <param name="length">The length of the string to generate.</param>
        /// <param name="includeSpecialChars">Whether to include special characters.</param>
        /// <returns>A random string of the specified length.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than or equal to zero.</exception>
        public static string CreateRandomString(int length, bool includeSpecialChars = false)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

            var chars = includeSpecialChars ? AllChars : AlphanumericChars;

            return string.Create(length, chars, static (span, chars) =>
            {
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = chars[Random.Shared.Next(chars.Length)];
                }
            });
        }

        /// <summary>
        /// Creates a GUID string.
        /// </summary>
        /// <returns>A GUID string in standard format.</returns>
        public static string CreateGuid() => Guid.NewGuid().ToString();
    }
}

