namespace CRM.Core.Extensions.XML
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XMLHelper
    {
        public const string RootElementName = "ROOT";
        public const string RootElementWrapper = "<ROOT>{0}</ROOT>";

        private static XmlSerializerNamespaces xmlNameSpace = GetXmlSerializerNamespaces();
        private static XmlWriterSettings xmlSettings = GetXmlWriterSettings();
        private static readonly Dictionary<string, XmlSerializer> SerializerCaches = new Dictionary<string, XmlSerializer>();

        public static string SerializeList<T>(IEnumerable<T> list)
        {
            if (list == null || !list.Any())
            {
                return string.Empty;
            }

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))
            {
                var serializer = XmlCacheCreate(list.GetType(), new XmlRootAttribute(RootElementName));
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
                var serializer = XmlCacheCreate(typeof(List<T>), new XmlRootAttribute(RootElementName));
                return (List<T>)serializer.Deserialize(stringReader);
            }
        }

        private static XmlSerializerNamespaces GetXmlSerializerNamespaces()
        {
            var xmlNameSpaces = new XmlSerializerNamespaces();
            xmlNameSpaces.Add("", "");
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

        public static XmlSerializer XmlCacheCreate(Type type, XmlRootAttribute root)
        {
            var key = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", type, root.ElementName);

            if (!SerializerCaches.ContainsKey(key))
            {
                SerializerCaches.Add(key, new XmlSerializer(type, root));
            }

            return SerializerCaches[key];
        }
    }
}