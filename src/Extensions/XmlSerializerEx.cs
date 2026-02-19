using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;

namespace Grondo.Extensions
{
    /// <summary>
    /// Provides extension methods for XML serialization and deserialization.
    /// </summary>
    public static class XmlSerializerEx
    {
        private static readonly ConcurrentDictionary<Type, XmlSerializer> _serializerCache = new();

        private static XmlSerializer GetSerializer(Type type) =>
            _serializerCache.GetOrAdd(type, t => new XmlSerializer(t));

        extension<T>(T entity) where T : class
        {
            /// <summary>
            /// Serializes the specified entity into an XML string.
            /// </summary>
            /// <returns>An XML string representation of the entity.</returns>
            /// <exception cref="ArgumentNullException">Thrown if the entity is null.</exception>
            public string Serialize()
            {
                ArgumentNullException.ThrowIfNull(entity);

                XmlSerializer serializer = GetSerializer(typeof(T));
                using var sw = new StringWriter();
                serializer.Serialize(sw, entity);

                return sw.ToString();
            }
        }

        extension(string xml)
        {
            /// <summary>
            /// Deserializes the specified XML string into an object of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type of object to deserialize into.</typeparam>
            /// <returns>
            /// An instance of type <typeparamref name="T"/> if deserialization succeeds;
            /// otherwise, <c>null</c>.
            /// </returns>
            public T? Deserialize<T>()
            {
                if (string.IsNullOrWhiteSpace(xml)) return default;

                XmlSerializer serializer = GetSerializer(typeof(T));
                using var sr = new StringReader(xml);
                using var reader = XmlReader.Create(sr, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });

                return (T?)serializer.Deserialize(reader);
            }
        }
    }
}
