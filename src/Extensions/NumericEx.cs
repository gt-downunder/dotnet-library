using System.Numerics;

namespace DotNet.Library.Extensions
{
    public static class NumericEx
    {
        public static bool IsZero<T>(this T number) where T : INumber<T> => number == T.Zero;
        public static bool IsPositive<T>(this T number) where T : INumber<T> => number > T.Zero;
        public static bool IsNegative<T>(this T number) where T : INumber<T> => number < T.Zero;
    }
}