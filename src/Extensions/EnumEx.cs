using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for enums, including description and display name retrieval.
    /// </summary>
    public static class EnumEx
    {
        extension(Enum value)
        {
            /// <summary>
            /// Retrieves the <see cref="DescriptionAttribute"/> value applied to an enum member, if present.
            /// </summary>
            /// <returns>
            /// The description specified in <see cref="DescriptionAttribute"/> if found;
            /// otherwise, the enum member's name as a string.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
            public string GetDescription()
            {
                ArgumentNullException.ThrowIfNull(value);

                FieldInfo? fi = value.GetType().GetField(value.ToString());

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
            /// <returns>
            /// The custom attribute of type <typeparamref name="T"/> if found;
            /// otherwise, <c>null</c>.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
            public T? GetCustomAttribute<T>() where T : Attribute
            {
                ArgumentNullException.ThrowIfNull(value);

                FieldInfo? fi = value.GetType().GetField(value.ToString());
                return fi?.GetCustomAttribute<T>();
            }

            /// <summary>
            /// Retrieves the string value specified by <see cref="EnumMemberAttribute"/> applied to an enum member.
            /// </summary>
            /// <returns>
            /// The string value defined in <see cref="EnumMemberAttribute"/> if present;
            /// otherwise, <c>null</c>.
            /// </returns>
            /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
            public string? GetEnumMemberValue()
            {
                ArgumentNullException.ThrowIfNull(value);
                return value.GetCustomAttribute<EnumMemberAttribute>()?.Value;
            }
        }
    }
}
