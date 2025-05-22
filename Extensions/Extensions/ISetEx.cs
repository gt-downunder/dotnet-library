using System;
using System.Collections.Generic;
using System.Text.Json;

namespace DotNet.Library.Extensions
{
    public static class ISetEx
    {
        public static bool SetEquals<T1>(this ISet<T1> set, string serializedSet)
        {
            if (string.IsNullOrWhiteSpace(serializedSet))
            {
                throw new ArgumentNullException(nameof(serializedSet));
            }

            var deserializedSet = JsonSerializer.Deserialize<IEnumerable<T1>>(serializedSet);

            if (deserializedSet == null)
            {
                return false;
            }

            return set.SetEquals(deserializedSet);
        }
    }
}
