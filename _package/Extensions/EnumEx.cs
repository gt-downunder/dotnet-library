using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace DotNet.Library.Extensions
{
    public static class EnumEx
    {
        /// <summary>
        /// Retrieves the <see cref="DescriptionAttribute"/> value applied to an enum member, if present.
        /// </summary>
        /// <param name="value">The enum member to inspect.</param>
        /// <returns>
        /// The description specified in <see cref="DescriptionAttribute"/> if found; 
        /// otherwise, the enum member's name as a string.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        public static string GetDescription(this Enum value)
        {
            ArgumentNullException.ThrowIfNull(value);

            var fi = value.GetType().GetField(value.ToString());

            return fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[]
            {
                Length: > 0
            } attributes
                ? attributes[0].Description
                : value.ToString();
        }

        /// <summary>
        /// Retrieves a custom attribute of type <typeparamref name="T"/> applied to an enum member.
        /// </summary>
        /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
        /// <param name="value">The enum member to inspect.</param>
        /// <returns>
        /// The custom attribute of type <typeparamref name="T"/> if found; 
        /// otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        public static T? GetCustomAttribute<T>(this Enum value) where T : Attribute
        {
            ArgumentNullException.ThrowIfNull(value);

            var fi = value.GetType().GetField(value.ToString());
            return fi?.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Retrieves the string value specified by <see cref="EnumMemberAttribute"/> applied to an enum member.
        /// </summary>
        /// <param name="value">The enum member to inspect.</param>
        /// <returns>
        /// The string value defined in <see cref="EnumMemberAttribute"/> if present; 
        /// otherwise, <c>null</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
        public static string? GetEnumMemberValue(this Enum value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return value.GetCustomAttribute<EnumMemberAttribute>()?.Value;
        }
    }
}