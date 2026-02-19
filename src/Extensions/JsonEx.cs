using System.Text.Json;
using Grondo.Utilities;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for JSON serialization and deserialization.
    /// </summary>
    public static class JsonEx
    {
        extension<T>(T obj)
        {
            /// <summary>
            /// Serializes the object to a JSON string.
            /// </summary>
            /// <param name="indented">Whether to format the JSON with indentation.</param>
            /// <returns>A JSON string representation of the object.</returns>
            public string ToJson(bool indented = false) =>
                JsonSerializer.Serialize(obj, indented ? JsonDefaults.Indented : JsonDefaults.Default);
        }

        extension(string json)
        {
            /// <summary>
            /// Deserializes a JSON string into an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type to deserialize into.</typeparam>
            /// <param name="caseInsensitive">Whether to use case-insensitive property matching.</param>
            /// <returns>An instance of <typeparamref name="T"/>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the JSON string is null.</exception>
            /// <exception cref="JsonException">Thrown if deserialization fails.</exception>
            public T? FromJson<T>(bool caseInsensitive = true)
            {
                ArgumentNullException.ThrowIfNull(json);
                return JsonSerializer.Deserialize<T>(json, caseInsensitive ? JsonDefaults.CaseInsensitive : JsonDefaults.Default);
            }
        }

        extension(string? json)
        {
            /// <summary>
            /// Attempts to deserialize a JSON string into an object of type <typeparamref name="T"/>.
            /// Returns <c>false</c> if deserialization fails.
            /// </summary>
            /// <typeparam name="T">The type to deserialize into.</typeparam>
            /// <param name="result">When this method returns, contains the deserialized object if successful.</param>
            /// <param name="caseInsensitive">Whether to use case-insensitive property matching.</param>
            /// <returns><c>true</c> if deserialization succeeded; otherwise, <c>false</c>.</returns>
            public bool TryFromJson<T>(out T? result, bool caseInsensitive = true)
            {
                result = default;
                if (string.IsNullOrWhiteSpace(json)) return false;

                try
                {
                    result = JsonSerializer.Deserialize<T>(json, caseInsensitive ? JsonDefaults.CaseInsensitive : JsonDefaults.Default);
                    return result is not null;
                }
                catch (JsonException)
                {
                    return false;
                }
            }
        }
    }
}

