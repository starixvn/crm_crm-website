namespace CRM.Models.NotificationTemplate
{
    using System;
    using Newtonsoft.Json;

    [Serializable]
    public class NotificationTemplateContent
    {
        [JsonIgnore]
        public int NotificationTemplateId { get; set; }

        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }
}