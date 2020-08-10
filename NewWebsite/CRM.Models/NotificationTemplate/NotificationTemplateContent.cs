namespace CRM.Models.NotificationTemplate
{
    using System;
    using CRM.Core.Extensions.XML;

    [Serializable]
    [XMLName("Row")]
    public class NotificationTemplateContent
    {
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }
}