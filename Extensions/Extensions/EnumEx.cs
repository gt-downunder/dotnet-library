using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace DotNet.Library.Extensions
{
    public static class EnumEx
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes?.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Retrieves an enum member's custom <see cref="Attribute"/>.
        /// </summary>
        /// <typeparam name="T">Attribute to retrieve.</typeparam>
        /// <param name="value">Source enum member.</param>
        /// <returns>Custom <see cref="Attribute"/> declared on enum member.</returns>
        public static T GetCustomAttribute<T>(this Enum value)
            where T : Attribute
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            return fi.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Retrieves the string value of an enum member.
        /// </summary>
        /// <param name="value">Source enum member.</param>
        /// <returns>String value declared on enum member.</returns>
        public static string GetEnumMemberValue(this Enum value)
        {
            return value.GetCustomAttribute<EnumMemberAttribute>().Value;
        }
    }
}
