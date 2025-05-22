using System;

namespace DotNet.Library.Extensions
{
    public static class NumericEx
    {
        public static bool IsZero<T>(this T number) where T : struct, IComparable => number.CompareTo(0) == 0;

        public static bool NotIsZero<T>(this T number) where T : struct, IComparable => number.CompareTo(0) != 0;

        public static bool IsPositive<T>(this T number) where T : struct, IComparable => number.CompareTo(0) > 0;

        public static bool NotIsPositive<T>(this T number) where T : struct, IComparable => number.CompareTo(0) <= 0;

        public static bool IsNegative<T>(this T number) where T : struct, IComparable => number.CompareTo(0) < 0;

        public static bool NotIsNegative<T>(this T number) where T : struct, IComparable => number.CompareTo(0) >= 0;
    }
}
