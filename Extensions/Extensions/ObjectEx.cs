using System;
using System.Collections;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DotNet.Library.Extensions
{
    public static class ObjectEx
    {
        public static bool IsNumeric(this object obj)
        {
            if (obj.IsNull() || obj is DateTime) { return false; }
            if (obj is short || obj is int || obj is long || obj is decimal || obj is float || obj is double) { return true; }

            try
            {
                if (obj is string)
                    double.Parse(obj as string);
                else
                    double.Parse(obj.ToString());

                return true;
            }
            catch { } // Swallow the error and then return false

            return false;
        }

        public static double ToDouble(this object obj)
        {
            double retValue = default;

            if (obj.IsNullOrEmpty()) { return retValue; }
            if (!double.TryParse(obj.ToString(), out retValue)) { return default; }

            return retValue;
        }

        public static int ToInteger(this object obj)
        {
            int retValue = default;

            if (obj.IsNullOrEmpty()) { return retValue; }
            if (!int.TryParse(obj.ToString(), out retValue)) { return default; }

            return retValue;
        }

        public static double? ToNullableDouble(this object obj)
        {
            if (obj.IsNullOrEmpty()) { return null; }
            if (obj.IsNumeric().IsFalse()) { return null; }

            return new double?(obj.ToDouble());
        }

        public static int? ToNullableInteger(this object obj)
        {
            if (obj.IsNullOrEmpty()) { return null; }
            if (obj.IsNumeric().IsFalse()) { return null; }

            return new int?(obj.ToInteger());
        }

        public static bool ToBoolean(this object obj)
        {
            bool retValue = default;

            if (obj.IsNullOrEmpty()) { return retValue; }
            if (!bool.TryParse(obj.ToString(), out retValue)) { return default; }

            return retValue;
        }

        public static bool? ToNullableBoolean(this object obj)
        {
            if (obj.IsNullOrEmpty()) { return null; }
            return new bool?(obj.ToBoolean());
        }

        public static DateTime ToDateTime(this object obj, string format = "")
        {
            if (obj.IsNullOrEmpty()) { return default; }

            return format.IsNullOrEmpty()
                ? DateTime.Parse(obj.ToString())
                : DateTime.ParseExact(obj.ToString(), format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToNullableDateTime(this object obj, string format = "")
        {
            if (obj.IsNullOrEmpty()) { return null; }
            return new DateTime?(obj.ToDateTime(format));
        }

        public static bool IsNullOrEmpty<T>(this T value)
        {
            return (value.IsNull())
                || value.Equals(default(T))
                || (value is IEnumerable enumerable && (enumerable).IsEmpty())
                || string.IsNullOrWhiteSpace(value.ToString());
        }

        public static T RunIfNullOrEmpty<T>(this T obj, Action action)
        {
            if (obj.IsNullOrEmpty()) { action(); }
            return obj;
        }

        public static bool IsNull<T>(this T value) => value == null;

        public static TResult RunIfNullOrEmpty<T, TResult>(this T obj, Func<TResult> func) => obj.IsNullOrEmpty() ? func() : default;

        public static T ReturnFromcodeIfNullOrEmpty<T>(this T obj, Func<T> func) => obj.IsNullOrEmpty() ? func() : obj;

        public static bool IsNotNullOrEmpty(this object obj) => obj.IsNullOrEmpty().IsFalse();

        public static bool IsNotNull(this object obj) => obj.IsNull().IsFalse();

        public static T RunIfNotNullOrEmpty<T>(this T obj, Action action)
        {
            if (obj.IsNotNullOrEmpty()) { action(); }
            return obj;
        }

        public static T RunIfNotNullOrEmpty<T>(this T obj, Action<T> action)
        {
            if (obj.IsNotNullOrEmpty()) { action(obj); }
            return obj;
        }

        public static TResult RunIfNotNullOrEmpty<T, TResult>(this T obj, Func<T, TResult> func)
        {
            return obj.IsNotNullOrEmpty() ? func(obj) : default;
        }

        public static TResult RunIfNotNullOrEmpty<T, TResult>(this T obj, Func<TResult> func)
        {
            return obj.IsNotNullOrEmpty() ? func() : default;
        }

        public static StringContent ToStringContent<T>(this T obj, string contentType)
        {
            if (obj.GetType() == typeof(string))
            {
                return new StringContent(obj.ToString(), Encoding.UTF8, contentType);
            }
            else
            {
                return JsonSerializer.Serialize(obj).ToStringContent(contentType);
            }
        }
    }
}
