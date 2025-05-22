using System.IO;
using System.Xml.Serialization;

namespace DotNet.Library.Extensions
{
    public static class XmlSerializerEx
    {
        public static string Serialize<T>(this T entity)
        {
            using StringWriter sw = new();
            new XmlSerializer(entity.GetType()).Serialize(sw, entity);

            return sw.ToString();
        }

        public static T Deserialize<T>(this T entity, string xml)
        {
            using StringReader sr = new(xml);
            return (T)new XmlSerializer(entity.GetType()).Deserialize(sr);
        }
    }
}
