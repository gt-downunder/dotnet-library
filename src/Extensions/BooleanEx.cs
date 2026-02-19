namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="bool"/>.
    /// </summary>
    public static class BooleanEx
    {
        extension(bool value)
        {
            /// <summary>
            /// Executes the specified <paramref name="action"/> if the boolean value is <c>true</c>.
            /// Returns the original boolean value for fluent chaining.
            /// </summary>
            /// <param name="action">The action to execute if the value is <c>true</c>.</param>
            /// <returns>The original boolean value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
            public bool RunIfTrue(Action action)
            {
                ArgumentNullException.ThrowIfNull(action);

                if (value) action();
                return value;
            }

            /// <summary>
            /// Executes the specified <paramref name="action"/> if the boolean value is <c>false</c>.
            /// Returns the original boolean value for fluent chaining.
            /// </summary>
            /// <param name="action">The action to execute if the value is <c>false</c>.</param>
            /// <returns>The original boolean value.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
            public bool RunIfFalse(Action action)
            {
                ArgumentNullException.ThrowIfNull(action);

                if (!value) action();
                return value;
            }
        }

        extension(bool? value)
        {
            /// <summary>
            /// Converts a nullable boolean to <c>false</c> if it is <c>null</c>.
            /// </summary>
            /// <returns><c>false</c> if the value is <c>null</c>; otherwise, the actual boolean value.</returns>
            public bool ToFalseIfNull() => value.GetValueOrDefault();
        }
    }
}
