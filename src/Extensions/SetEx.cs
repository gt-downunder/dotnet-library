using System.Text.Json;
using Grondo.Utilities;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="ISet{T}"/>.
    /// </summary>
    public static class SetEx
    {
        extension<T>(ISet<T> set)
        {
            /// <summary>
            /// Compares this set with a JSON-serialized set string.
            /// Returns true if they contain the same elements.
            /// </summary>
            /// <param name="json">A JSON string representing another set.</param>
            /// <returns><c>true</c> if the sets contain the same elements; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the set or <paramref name="json"/> is null or whitespace.</exception>
            public bool EqualsSerializedSet(string json)
            {
                ArgumentNullException.ThrowIfNull(set);
                if (string.IsNullOrWhiteSpace(json))
                    throw new ArgumentNullException(nameof(json));

                try
                {
                    HashSet<T>? other = JsonSerializer.Deserialize<HashSet<T>>(json, JsonDefaults.CaseInsensitive);
                    return other != null && set.SetEquals(other);
                }
                catch (JsonException)
                {
                    return false; // invalid JSON
                }
            }
        }
    }
}

