using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Library.Extensions
{
    public static class IEnumerableEx
    {
        public static bool IsEmpty(this IEnumerable value) => !value.NotIsEmpty();

        public static bool NotIsEmpty(this IEnumerable value)
        {
            IEnumerator enumerator = value.GetEnumerator();
            try
            {
                return enumerator.MoveNext();
            }
            finally
            {
                enumerator.Reset();
            }
        }

        /// <summary>
        /// Returns true if the two sequences contain the exact same elements, order withstanding (i.e. not mattering).
        /// </summary>
        /// <typeparam name="T">Type of the sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="target">Target sequence.</param>
        public static bool IsDeepEqualTo<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            return source.Count() == target.Count() && !source.Except(target).Any();
        }

        /// <summary>
        /// Iterate through an enumerable and execute a delegate action with each item and its index parameters.
        /// </summary>
        /// <typeparam name="T">Type of the sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Delegate action to execute.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                action(item, index++);
            }
        }

        /// <summary>
        /// Iterate through an enumerable and execute a delegate function with each item and its index parameters.
        /// Returns true if you want to break the loop or all items have been iterated through.
        /// </summary>
        /// <typeparam name="T">Type of the sequence elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="func">Delegate function to execute.</param>
        public static bool ForEach<T>(this IEnumerable<T> source, Func<T, int, bool> func)
        {
            int index = 0;
            foreach (T item in source)
            {
                if (!func(item, index++)) return false;
            }

            return true;
        }
    }
}
