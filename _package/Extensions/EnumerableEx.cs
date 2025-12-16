namespace DotNet.Library.Extensions
{
    public static class EnumerableEx
    {
        /// <summary>
        /// Determines whether the specified sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if the sequence is empty; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            ArgumentNullException.ThrowIfNull(source);
            using IEnumerator<T> enumerator = source.GetEnumerator();
            return !enumerator.MoveNext();
        }

        /// <summary>
        /// Determines whether the specified sequence contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="source">The source sequence to check.</param>
        /// <returns><c>true</c> if the sequence contains one or more elements; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is null.</exception>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source) => !source.IsEmpty();

        /// <summary>
        /// Determines whether two sequences contain the same elements in the same order.
        /// </summary>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="target">The target sequence to compare with.</param>
        /// <returns><c>true</c> if the sequences are equal; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="source"/> or <paramref name="target"/> is null.</exception>
        public static bool IsDeepEqualTo<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(target);
            return source.SequenceEqual(target);
        }

        /// <summary>
        /// Iterates through a sequence and executes the specified action for each element,
        /// providing both the element and its index.
        /// </summary>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="action">The action to execute for each element and its index.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="action"/> is null.</exception>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);

            var index = 0;
            foreach (var item in source)
            {
                action(item, index++);
            }
        }

        /// <summary>
        /// Iterates through a sequence and executes the specified function for each element,
        /// providing both the element and its index. Iteration continues until the function
        /// returns <c>false</c> or all elements have been processed.
        /// </summary>
        /// <typeparam name="T">The type of the sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="func">The function to execute for each element and its index. 
        /// Return <c>false</c> to break the iteration early.</param>
        /// <returns><c>true</c> if all elements were processed without the function returning <c>false</c>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> or <paramref name="func"/> is null.</exception>
        public static bool ForEach<T>(this IEnumerable<T> source, Func<T, int, bool> func)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(func);

            var index = 0;
            return source.All(item => func(item, index++));
        }
    }
}