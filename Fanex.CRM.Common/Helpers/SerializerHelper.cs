using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Fanex.CRM.Common.Helpers
{
    public static class SerializerHelper
    {
        public const string RootElementName = "root";
        public const string RootElementWrapper = "<root>{0}</root>";

        private static XmlSerializerNamespaces xmlNameSpace = GetXmlSerializerNamespaces();
        private static XmlWriterSettings xmlSettings = GetXmlWriterSettings();


        public static string SerializeList<T>(IEnumerable<T> list)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
            {
                var serializer = XmlSerializerCache.Create(list.GetType(), new XmlRootAttribute(RootElementName));
                serializer.Serialize(xmlWriter, list, xmlNameSpace);

                return stringWriter.ToString();
            }
        }

        public static IList<T> DeserializeList<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new List<T>();
            }

            using (var stringReader = new StringReader(xml))
            {
                var serializer = XmlSerializerCache.Create(typeof(List<T>), new XmlRootAttribute(RootElementName));
                return (List<T>)serializer.Deserialize(stringReader);
            }
        }

        private static XmlSerializerNamespaces GetXmlSerializerNamespaces()
        {
            var xmlNameSpaces = new XmlSerializerNamespaces();
            xmlNameSpaces.Add(string.Empty, string.Empty);
            return xmlNameSpaces;
        }

        private static XmlWriterSettings GetXmlWriterSettings()
        {
            return new XmlWriterSettings
            {
                Indent = false,
                OmitXmlDeclaration = true,
                NewLineHandling = NewLineHandling.None
            };
        }
    }
}
