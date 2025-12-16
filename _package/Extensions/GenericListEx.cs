namespace DotNet.Library.Extensions
{
    public static class GenericListEx
    {
        /// <summary>
        /// Adds one or more items to the list and returns the list for fluent chaining.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to add items to.</param>
        /// <param name="items">The items to add.</param>
        /// <returns>The same list instance with the new items added.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> or <paramref name="items"/> is null.</exception>
        public static List<T> Add<T>(this List<T> list, params T[] items)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(items);

            list.AddRange(items);
            return list;
        }

        /// <summary>
        /// Adds a value to the list if the value is not null.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to add the value to.</param>
        /// <param name="value">The value to add if not null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is null.</exception>
        public static void AddIfNotNull<T>(this List<T> list, T value)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (value is not null)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Adds a string to the list if it does not already exist (case-insensitive).
        /// </summary>
        /// <param name="list">The list to add the string to.</param>
        /// <param name="value">The string value to add if not already present.</param>
        /// <returns>The same list instance with the new value added if it was not present.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> or <paramref name="value"/> is null.</exception>
        public static List<string> AddIfNotExists(this List<string> list, string value)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(value);

            if (!list.ContainsIgnoreCase(value))
            {
                list.Add(value);
            }

            return list;
        }

        /// <summary>
        /// Adds a range of strings to the list, ensuring no duplicates are added (case-insensitive).
        /// </summary>
        /// <param name="list">The list to add the strings to.</param>
        /// <param name="range">The range of strings to add.</param>
        /// <returns>The same list instance with the new values added if they were not already present.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> is null.</exception>
        public static List<string> AddRangeNoDuplicates(this List<string> list, IEnumerable<string> range)
        {
            ArgumentNullException.ThrowIfNull(list);

            IEnumerable<string> enumerable = range as string[] ?? range.ToArray();
            if (!enumerable.Any())
            {
                return list;
            }

            foreach (var item in enumerable)
            {
                list.AddIfNotExists(item);
            }

            return list;
        }

        /// <summary>
        /// Determines whether the list contains the specified string using a case-insensitive comparison.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="toCompare">The string value to compare.</param>
        /// <returns><c>true</c> if the list contains the specified string (case-insensitive); otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="list"/> or <paramref name="toCompare"/> is null.</exception>
        public static bool ContainsIgnoreCase(this List<string> list, string toCompare)
        {
            ArgumentNullException.ThrowIfNull(list);
            ArgumentNullException.ThrowIfNull(toCompare);

            return list.Any(x => x.Equals(toCompare, StringComparison.OrdinalIgnoreCase));
        }
    }
}