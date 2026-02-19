namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableEx
    {
        extension<T>(IEnumerable<T>? source)
        {
            /// <summary>
            /// Determines whether the specified sequence is <c>null</c> or contains no elements.
            /// </summary>
            /// <returns><c>true</c> if the sequence is <c>null</c> or empty; otherwise, <c>false</c>.</returns>
            public bool IsNullOrEmpty() =>
                source?.Any() != true;

            /// <summary>
            /// Determines whether the specified sequence is not <c>null</c> and contains at least one element.
            /// </summary>
            /// <returns><c>true</c> if the sequence is not <c>null</c> and contains elements; otherwise, <c>false</c>.</returns>
            public bool IsNotNullOrEmpty() =>
                source?.Any() == true;
        }

        extension<T>(IEnumerable<T> source)
        {
            /// <summary>
            /// Determines whether the specified sequence contains no elements.
            /// </summary>
            /// <returns><c>true</c> if the sequence is empty; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
            public bool IsEmpty()
            {
                ArgumentNullException.ThrowIfNull(source);
                return !source.Any();
            }

            /// <summary>
            /// Determines whether the specified sequence contains at least one element.
            /// </summary>
            /// <returns><c>true</c> if the sequence contains one or more elements; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
            public bool IsNotEmpty() => !source.IsEmpty();

            /// <summary>
            /// Determines whether two sequences contain the same elements in the same order.
            /// </summary>
            /// <param name="target">The target sequence to compare with.</param>
            /// <returns><c>true</c> if the sequences are equal; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if either the source or <paramref name="target"/> is null.</exception>
            public bool IsDeepEqualTo(IEnumerable<T> target)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(target);
                return source.SequenceEqual(target);
            }

            /// <summary>
            /// Iterates through a sequence and executes the specified action for each element,
            /// providing both the element and its index.
            /// </summary>
            /// <param name="action">The action to execute for each element and its index.</param>
            /// <exception cref="ArgumentNullException">Thrown if the source or <paramref name="action"/> is null.</exception>
            public void ForEach(Action<T, int> action)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(action);

                int index = 0;
                foreach (T? item in source)
                {
                    action(item, index++);
                }
            }

            /// <summary>
            /// Iterates through a sequence and executes the specified function for each element,
            /// providing both the element and its index. Iteration continues until the function
            /// returns <c>false</c> or all elements have been processed.
            /// </summary>
            /// <param name="func">The function to execute for each element and its index.
            /// Return <c>false</c> to break the iteration early.</param>
            /// <returns><c>true</c> if all elements were processed without the function returning <c>false</c>; otherwise, <c>false</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the source or <paramref name="func"/> is null.</exception>
            public bool ForEach(Func<T, int, bool> func)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(func);

                int index = 0;
                foreach (T? item in source)
                {
                    if (!func(item, index++)) return false;
                }

                return true;
            }

            /// <summary>
            /// Splits the sequence into batches of the specified size.
            /// The last batch may contain fewer elements.
            /// </summary>
            /// <param name="batchSize">The maximum number of elements per batch.</param>
            /// <returns>A sequence of batches, each containing up to <paramref name="batchSize"/> elements.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
            /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="batchSize"/> is less than 1.</exception>
            public IEnumerable<IReadOnlyList<T>> Batch(int batchSize)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentOutOfRangeException.ThrowIfLessThan(batchSize, 1);

                var batch = new List<T>(batchSize);
                foreach (T item in source)
                {
                    batch.Add(item);
                    if (batch.Count == batchSize)
                    {
                        yield return batch;
                        batch = new List<T>(batchSize);
                    }
                }

                if (batch.Count > 0)
                    yield return batch;
            }

            /// <summary>
            /// Splits the sequence into two groups based on a predicate.
            /// </summary>
            /// <param name="predicate">The predicate to evaluate for each element.</param>
            /// <returns>A tuple of (Matches, NonMatches) where Matches contains elements for which the predicate returned <c>true</c>.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the source or <paramref name="predicate"/> is null.</exception>
            public (IReadOnlyList<T> Matches, IReadOnlyList<T> NonMatches) Partition(Func<T, bool> predicate)
            {
                ArgumentNullException.ThrowIfNull(source);
                ArgumentNullException.ThrowIfNull(predicate);

                var matches = new List<T>();
                var nonMatches = new List<T>();

                foreach (T item in source)
                {
                    if (predicate(item))
                        matches.Add(item);
                    else
                        nonMatches.Add(item);
                }

                return (matches, nonMatches);
            }

        }

        extension<T>(IEnumerable<T?> source)
        {
            /// <summary>
            /// Filters out <c>null</c> values from the sequence.
            /// </summary>
            /// <returns>A sequence containing only non-null elements.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
            public IEnumerable<T> WhereNotNull()
            {
                ArgumentNullException.ThrowIfNull(source);

                foreach (T? item in source)
                {
                    if (item is not null)
                        yield return item;
                }
            }
        }
    }
}
