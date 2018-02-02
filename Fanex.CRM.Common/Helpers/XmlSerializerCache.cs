using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Fanex.CRM.Common.Helpers
{
    public static class XmlSerializerCache
    {
        private static readonly Dictionary<string, XmlSerializer> SerializerCaches = new Dictionary<string, XmlSerializer>();

        public static XmlSerializer Create(Type type, XmlRootAttribute root)
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
