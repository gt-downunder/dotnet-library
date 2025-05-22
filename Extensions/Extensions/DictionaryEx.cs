using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace DotNet.Library.Extensions
{
    public static class DictionaryEx
    {
        public static string ToJson<K, V>(this IDictionary<K, V> source)
        {
            string serialized = string.Empty;
            var serializer = new DataContractJsonSerializer(source.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, source);
                serialized = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return serialized;
        }

        public static bool HasKeyAndValue<K, V>(this IDictionary<K, V> source, K key)
        {
            return source.ContainsKey(key) && source[key].IsNotNull();
        }

        /// <summary>
        /// Checks the dictionary for any one of the specified keys
        /// </summary>
        /// <typeparam name="K">Type of key</typeparam>
        /// <typeparam name="V">Type of value</typeparam>
        /// <param name="source">Dictionary to act upon.</param>
        /// <param name="keyList">Array of keys to search for</param>
        /// <returns>True if the any one of the keys is present in the dictionary, otherwise false</returns>
        public static bool HasAnyKey<K, V>(this IDictionary<K, V> source, K[] keyList)
        {
            if (keyList.IsNotNull())
            {
                foreach (var key in keyList)
                {
                    if (key.IsNotNull() && source.ContainsKey(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to retrieve the value of the specified key from the dictionary
        /// </summary>
        /// <typeparam name="K">Type of key</typeparam>
        /// <typeparam name="V">Type of value</typeparam>
        /// <param name="source">Dictionary to act upon.</param>
        /// <param name="key">Key to search for</param>
        /// <returns>The value of the specified key if it exists, a default instance of the value if it doesn't</returns>
        public static V TryGetValue<K, V>(this IDictionary<K, V> source, K key)
        {
            return key.IsNotNull() && source.TryGetValue(key, out V value) ? value : (default);
        }

        /// <summary>
        /// Adds a key-value entry into a serialized nested dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to act upon.</param>
        /// <param name="fieldName">Key containing nested dictionary.</param>
        /// <param name="key">Key to add.</param>
        /// <param name="value">Value to add.</param>
        public static void AddEntryToNestedDictionary(this IDictionary<string, string> dict, string fieldName, string key, string value)
        {
            Dictionary<string, string> links;
            if (string.IsNullOrWhiteSpace(dict[fieldName]))
            {
                // need to initialize nested dictionary
                links = [];
            }
            else
            {
                links = JsonSerializer.Deserialize<Dictionary<string, string>>(dict[fieldName]);
            }

            links.Add(key, value);

            dict[fieldName] = JsonSerializer.Serialize(links);
        }

        /// <summary>
        /// Removes a key-value entry from a serialized nested dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to act upon.</param>
        /// <param name="fieldName">Key containing nested dictionary.</param>
        /// <param name="key">Key to remove.</param>
        public static void RemoveEntryFromNestedDictionary(this Dictionary<string, string> dict, string fieldName, string key)
        {
            if (string.IsNullOrWhiteSpace(dict[fieldName]))
            {
                // nested dictionary doesn't exist so no-op
            }
            else
            {
                var links = JsonSerializer.Deserialize<Dictionary<string, string>>(dict[fieldName]);

                links.Remove(key);

                dict[fieldName] = JsonSerializer.Serialize(links);
            }
        }

        /// <summary>
        /// Checks if two dictionaries contain all the same key-value pairs.
        /// </summary>
        /// <param name="dict">Source dictionary.</param>
        /// <param name="targetDictionary">Target dictionary.</param>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <returns>True if the source and target dictionaries are the same.</returns>
        public static bool IsDeepEqualTo<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> targetDictionary)
        {
            return dict.Count == targetDictionary.Count && !dict.Except(targetDictionary).Any();
        }

        /// <summary>
        /// Checks if a string representing a serialized Dictionary has all the same key-value pairs.
        /// </summary>
        /// <remarks>Useful for testing because usually we don't care about dictionary order.</remarks>
        /// <param name="dict">Source dictionary.</param>
        /// <param name="serializedDictionary">Target dictionary.</param>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <returns>True if the source and target dictionaries are the same.</returns>
        public static bool IsDeepEqualTo<TKey, TValue>(this Dictionary<TKey, TValue> dict, string serializedDictionary)
        {
            if (string.IsNullOrWhiteSpace(serializedDictionary))
            {
                throw new ArgumentNullException(nameof(serializedDictionary));
            }

            var deserializedDict = JsonSerializer.Deserialize<Dictionary<TKey, TValue>>(serializedDictionary);

            if (deserializedDict == null)
            {
                return false;
            }

            return dict.IsDeepEqualTo(deserializedDict);
        }

        /// <summary>
        /// "Pretty-print" a dictionary.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="dictionary">Source dictionary.</param>
        /// <returns>Pretty-print string representation of dictionary.</returns>
        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) =>
            "{" + string.Join(", ", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";

        /// <summary>
        /// URL querystring representation of a dictionary.
        /// </summary>
        /// <typeparam name="K">Key type.</typeparam>
        /// <typeparam name="V">Value type.</typeparam>
        /// <param name="source">Source dictionary.</param>
        /// <returns>All key-value pairs converted into a querystring.</returns>
        public static string ToQueryString<K, V>(this IDictionary<K, V> source)
        {
            var array = source.Select(kvPair => $"{kvPair.Key}={kvPair.Value}");
            if (array.Any())
            {
                return $"?{string.Join("&", array)}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
