namespace Grondo.Utilities
{
    /// <summary>
    /// Provides factory methods for generating string values.
    /// </summary>
    public static class StringFactory
    {
        /// <summary>
        /// Generates a unique string identifier using a GUID in "N" format (32 hex digits, no hyphens).
        /// </summary>
        public static string UniqueString => Guid.NewGuid().ToString("N");
    }
}

