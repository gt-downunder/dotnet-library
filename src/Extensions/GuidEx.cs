namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Guid"/>.
    /// </summary>
    public static class GuidEx
    {
        extension(Guid guid)
        {
            /// <summary>
            /// Determines whether the <see cref="Guid"/> is equal to <see cref="Guid.Empty"/>.
            /// </summary>
            /// <returns><c>true</c> if the GUID is empty; otherwise, <c>false</c>.</returns>
            public bool IsEmpty() =>
                guid == Guid.Empty;

            /// <summary>
            /// Converts the <see cref="Guid"/> to a compact 22-character Base64-encoded string
            /// without padding characters.
            /// </summary>
            /// <returns>A 22-character URL-safe string representation of the GUID.</returns>
            public string ToShortString() =>
                Convert.ToBase64String(guid.ToByteArray())
                    .Replace("/", "_", StringComparison.Ordinal)
                    .Replace("+", "-", StringComparison.Ordinal)
                    .TrimEnd('=');
        }
    }
}

