using System.Text.Json;

namespace DotNet.Library.Extensions
{
    public static class DictionaryEx
    {
        /// <summary>
        /// Serializes the dictionary into a JSON string.
        /// </summary>
        /// <typeparam name="TK">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TV">The type of values in the dictionary.</typeparam>
        /// <param name="source">The dictionary to serialize.</param>
        /// <returns>A JSON string representation of the dictionary.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
        public static string ToJson<TK, TV>(this IDictionary<TK, TV> source) =>
            JsonSerializer.Serialize(source ?? throw new ArgumentNullException(nameof(source)));

        /// <summary>
        /// Determines whether the dictionary contains the specified key and its value is not null.
        /// </summary>
        /// <typeparam name="TK">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TV">The type of values in the dictionary.</typeparam>
        /// <param name="source">The dictionary to check.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns><c>true</c> if the dictionary contains the key and its value is not null; otherwise, <c>false</c>.</returns>
        public static bool HasKeyAndValue<TK, TV>(this IDictionary<TK, TV> source, TK key) =>
            source.ContainsKey(key) && source[key] is not null;

        /// <summary>
        /// Determines whether the dictionary contains any of the specified keys.
        /// </summary>
        /// <typeparam name="TK">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TV">The type of values in the dictionary.</typeparam>
        /// <param name="source">The dictionary to check.</param>
        /// <param name="keyList">An array of keys to search for.</param>
        /// <returns><c>true</c> if any of the keys are present in the dictionary; otherwise, <c>false</c>.</returns>
        public static bool HasAnyKey<TK, TV>(this IDictionary<TK, TV> source, TK[]? keyList) =>
            keyList != null && keyList.Any(source.ContainsKey);

        /// <summary>
        /// Retrieves the value associated with the specified key, or the default value if the key is not found.
        /// </summary>
        /// <typeparam name="TK">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TV">The type of values in the dictionary.</typeparam>
        /// <param name="source">The dictionary to search.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>The value associated with the key if found; otherwise, the default value of <typeparamref name="TV"/>.</returns>
        public static TV? GetValueOrDefault<TK, TV>(this IDictionary<TK, TV?> source, TK key) =>
            source.TryGetValue(key, out var value) ? value : default;

        /// <summary>
        /// Adds a key-value pair to a nested dictionary stored as a JSON string within the parent dictionary.
        /// </summary>
        /// <param name="dict">The parent dictionary containing the nested dictionary as a JSON string.</param>
        /// <param name="fieldName">The key in the parent dictionary that stores the nested dictionary.</param>
        /// <param name="key">The key to add to the nested dictionary.</param>
        /// <param name="value">The value to add to the nested dictionary.</param>
        public static void AddEntryToNestedDictionary(this IDictionary<string, string> dict, string fieldName, string key, string value)
        {
            Dictionary<string, string> links = string.IsNullOrWhiteSpace(dict[fieldName])
                ? new Dictionary<string, string>()
                : JsonSerializer.Deserialize<Dictionary<string, string>>(dict[fieldName]) ??
                  new Dictionary<string, string>();

            links[key] = value;
            dict[fieldName] = JsonSerializer.Serialize(links);
        }

        /// <summary>
        /// Removes a key-value pair from a nested dictionary stored as a JSON string within the parent dictionary.
        /// </summary>
        /// <param name="dict">The parent dictionary containing the nested dictionary as a JSON string.</param>
        /// <param name="fieldName">The key in the parent dictionary that stores the nested dictionary.</param>
        /// <param name="key">The key to remove from the nested dictionary.</param>
        public static void RemoveEntryFromNestedDictionary(this IDictionary<string, string> dict, string fieldName, string key)
        {
            if (string.IsNullOrWhiteSpace(dict[fieldName])) return;

            var links = JsonSerializer.Deserialize<Dictionary<string, string>>(dict[fieldName]);
            if (links == null) return;

            links.Remove(key);
            dict[fieldName] = JsonSerializer.Serialize(links);
        }

        /// <summary>
        /// Determines whether two dictionaries contain the same key-value pairs.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionaries.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionaries.</typeparam>
        /// <param name="dict">The source dictionary.</param>
        /// <param name="other">The dictionary to compare with.</param>
        /// <returns><c>true</c> if both dictionaries contain the same key-value pairs; otherwise, <c>false</c>.</returns>
        public static bool IsDeepEqualTo<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> other) where TKey : notnull =>
            dict.Count == other.Count && dict.All(kv =>
                other.TryGetValue(kv.Key, out var val) && EqualityComparer<TValue>.Default.Equals(kv.Value, val));

        /// <summary>
        /// Determines whether the dictionary contains the same key-value pairs as a serialized dictionary string.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dict">The source dictionary.</param>
        /// <param name="serializedDictionary">A JSON string representing the target dictionary.</param>
        /// <returns><c>true</c> if both dictionaries contain the same key-value pairs; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="serializedDictionary"/> is null or whitespace.</exception>
        public static bool IsDeepEqualTo<TKey, TValue>(this IDictionary<TKey, TValue> dict, string serializedDictionary) where TKey : notnull
        {
            if (string.IsNullOrWhiteSpace(serializedDictionary))
                throw new ArgumentNullException(nameof(serializedDictionary));

            var deserialized = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(serializedDictionary);
            return deserialized != null && dict.IsDeepEqualTo(deserialized);
        }

        /// <summary>
        /// Returns a string representation of the dictionary in a debug-friendly format.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to format.</param>
        /// <returns>A string representation of the dictionary in the form "{key=value, ...}".</returns>
        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) =>
            "{" + string.Join(", ", dictionary.Select(kv => $"{kv.Key}={kv.Value}")) + "}";

        /// <summary>
        /// Converts the dictionary into a URL query string.
        /// </summary>
        /// <typeparam name="TK">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TV">The type of values in the dictionary.</typeparam>
        /// <param name="source">The dictionary to convert.</param>
        /// <returns>A query string representation of the dictionary, or an empty string if the dictionary is empty.</returns>
        public static string ToQueryString<TK, TV>(this IDictionary<TK, TV> source) =>
            source.Any()
                ? "?" + string.Join("&", source.Select(kv =>
                    $"{Uri.EscapeDataString(kv.Key?.ToString() ?? string.Empty)}={Uri.EscapeDataString(kv.Value?.ToString() ?? string.Empty)}"))
                : string.Empty;
    }
}