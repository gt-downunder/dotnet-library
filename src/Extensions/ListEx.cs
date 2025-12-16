namespace DotNet.Library.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class ListEx
    {
        /// <summary>
        /// Adds an item to the list if the item is not <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to add the item to. May be <c>null</c>.</param>
        /// <param name="item">The item to add if not <c>null</c>.</param>
        public static void AddIfNotNull<T>(this IList<T>? list, T item)
        {
            if (item is null) return;
            list?.Add(item);
        }

        /// <summary>
        /// Adds a range of items to the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The destination list for the items.</param>
        /// <param name="items">The source items to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> or <paramref name="items"/> is <c>null</c>.</exception>
        public static void AddRange<T>(this IList<T> list, params T[] items)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(items);

            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Adds a range of items to the list, skipping any items that are <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The destination list for the items.</param>
        /// <param name="items">The source items to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> or <paramref name="items"/> is <c>null</c>.</exception>
        public static void AddRangeIfNotNull<T>(this IList<T> list, params T[] items)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(items);

            foreach (var item in items)
            {
                list.AddIfNotNull(item);
            }
        }
    }
}