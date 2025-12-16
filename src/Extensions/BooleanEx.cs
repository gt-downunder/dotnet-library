namespace DotNet.Library.Extensions
{
    using System;

    public static class BooleanEx
    {
        /// <summary>
        /// Executes the specified <paramref name="action"/> if the boolean value is <c>true</c>.
        /// Returns the original boolean value for fluent chaining.
        /// </summary>
        /// <param name="value">The boolean value to evaluate.</param>
        /// <param name="action">The action to execute if <paramref name="value"/> is <c>true</c>.</param>
        /// <returns>The original boolean value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
        public static bool RunIfTrue(this bool value, Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (value) action();
            return value;
        }

        /// <summary>
        /// Executes the specified <paramref name="action"/> if the boolean value is <c>false</c>.
        /// Returns the original boolean value for fluent chaining.
        /// </summary>
        /// <param name="value">The boolean value to evaluate.</param>
        /// <param name="action">The action to execute if <paramref name="value"/> is <c>false</c>.</param>
        /// <returns>The original boolean value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is null.</exception>
        public static bool RunIfFalse(this bool value, Action action)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (!value) action();
            return value;
        }

        /// <summary>
        /// Converts a nullable boolean to <c>false</c> if it is <c>null</c>.
        /// </summary>
        /// <param name="value">The nullable boolean value.</param>
        /// <returns><c>false</c> if <paramref name="value"/> is <c>null</c>; otherwise, the actual boolean value.</returns>
        public static bool ToFalseIfNull(this bool? value) => value.GetValueOrDefault();
    }
}