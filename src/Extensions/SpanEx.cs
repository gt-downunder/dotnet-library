using System.Globalization;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="ReadOnlySpan{T}"/> of <see cref="char"/>.
    /// </summary>
    public static class SpanEx
    {
        extension(ReadOnlySpan<char> span)
        {
            /// <summary>
            /// Determines whether the span contains the specified value using a case-insensitive comparison.
            /// </summary>
            /// <param name="value">The value to search for.</param>
            /// <returns><c>true</c> if the span contains the value (case-insensitive); otherwise, <c>false</c>.</returns>
            public bool ContainsIgnoreCase(ReadOnlySpan<char> value) =>
                span.Contains(value, StringComparison.OrdinalIgnoreCase);

            /// <summary>
            /// Determines whether the span represents a valid numeric value.
            /// </summary>
            /// <returns><c>true</c> if the span can be parsed as a <see cref="double"/>; otherwise, <c>false</c>.</returns>
            public bool IsNumeric() =>
                !span.IsEmpty && double.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out _);

            /// <summary>
            /// Returns a trimmed slice of the span, or an empty span if the input is empty.
            /// Equivalent to <see cref="MemoryExtensions.Trim(ReadOnlySpan{char})"/> but safe on empty spans.
            /// </summary>
            /// <returns>A trimmed slice of the span.</returns>
            public ReadOnlySpan<char> SafeTrim() =>
                span.IsEmpty ? span : span.Trim();
        }
    }
}

