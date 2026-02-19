using System.Numerics;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for numeric types implementing <see cref="System.Numerics.INumber{T}"/>.
    /// </summary>
    public static class NumericEx
    {
        extension<T>(T number) where T : INumber<T>
        {
            /// <summary>Determines whether the numeric value is equal to zero.</summary>
            public bool IsZero() => number == T.Zero;

            /// <summary>Determines whether the numeric value is greater than zero.</summary>
            public bool IsPositive() => number > T.Zero;

            /// <summary>Determines whether the numeric value is less than zero.</summary>
            public bool IsNegative() => number < T.Zero;
        }
    }
}

