using System;

namespace DotNet.Library.Extensions
{
    public static class BooleanEx
    {
        public static void RunIfTrue(this bool value, Action action)
        {
            if (value) { action(); }
        }

        public static void RunIfFalse(this bool value, Action action)
        {
            (!value).RunIfTrue(action);
        }

        public static bool ToFalseIfNull(this bool? value)
        {
            return value ?? false;
        }

        public static bool IsFalse(this bool value) => !value;

        public static bool IsTrue(this bool value) => value;
    }
}
