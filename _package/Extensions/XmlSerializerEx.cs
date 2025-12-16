using System.Xml.Serialization;

namespace DotNet.Library.Extensions
{
    public static class XmlSerializerEx
    {
        /// <summary>
        /// Serializes the specified entity into an XML string.
        /// </summary>
        /// <typeparam name="T">The type of the entity to serialize.</typeparam>
        /// <param name="entity">The entity instance to serialize.</param>
        /// <returns>An XML string representation of the entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entity"/> is null.</exception>
        public static string Serialize<T>(this T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var serializer = new XmlSerializer(typeof(T));
            using var sw = new StringWriter();
            serializer.Serialize(sw, entity);

            return sw.ToString();
        }

        /// <summary>
        /// Deserializes the specified XML string into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize into.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>
        /// An instance of type <typeparamref name="T"/> if deserialization succeeds; 
        /// otherwise, <c>null</c>.
        /// </returns>
        public static T? Deserialize<T>(this string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return default;

            var serializer = new XmlSerializer(typeof(T));
            using var sr = new StringReader(xml);

            return (T?)serializer.Deserialize(sr);
        }
    }
}