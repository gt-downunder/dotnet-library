using System.Collections.Generic;

namespace DotNet.Library.Extensions
{
    public static class IListEx
    {
        public static void AddIfNotNull<T>(this IList<T> list, T item)
        {
            if (item.IsNull()) { return; }
            if (list.IsNull()) { return; }

            list.Add(item);
        }

        /// <summary>
        /// Add a range of items to an IList
        /// </summary>
        /// <param name="list">Destination list for the items</param>
        /// <param name="items">Source items to add</param>
        public static void AddRange<T>(this IList<T> list, params T[] items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Add a range of items to an IList but only when the item is not null
        /// </summary>
        /// <param name="list">Destination list for the items</param>
        /// <param name="items">Source items to add</param>
        public static void AddRangeIfNotNull<T>(this IList<T> list, params T[] items)
        {
            foreach (var item in items)
            {
                list.AddIfNotNull(item);
            }
        }
    }
}
