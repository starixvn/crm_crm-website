namespace CRM.Models.NotificationTemplate
{
    using System;
    using System.Xml.Serialization;
    using CRM.Core.Extensions.XML;
    using Newtonsoft.Json;

    [Serializable]
    [XMLName("Content")]
    public class NotificationTemplateContent
    {
        [JsonIgnore]
        [XmlIgnore]
        public int NotificationTemplateId { get; set; }

        public int LanguageId { get; set; }

        [XmlIgnore]
        public string Language { get; set; }

        [XMLName("Value")]
        public string Content { get; set; }
    }
}