using System.Text.Json;

namespace DotNet.Library.Extensions
{
    public static class SetEx
    {
        /// <summary>
        /// Compares this set with a JSON-serialized set string.
        /// Returns true if they contain the same elements.
        /// </summary>
        public static bool EqualsSerializedSet<T>(this ISet<T> set, string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));

            try
            {
                var other = JsonSerializer.Deserialize<HashSet<T>>(json);
                return other != null && set.SetEquals(other);
            }
            catch (JsonException)
            {
                return false; // invalid JSON
            }
        }
    }
}