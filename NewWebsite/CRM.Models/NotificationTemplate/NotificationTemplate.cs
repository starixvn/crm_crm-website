namespace CRM.Models.NotificationTemplate
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Serializable]
    public class NotificationTemplate
    {
        public int NotificationTemplateId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public int NotificationTypeId { get; set; }
        public string NotificationType { get; set; }

        [JsonIgnore]
        public string ContentXml { get; set; }

        public IEnumerable<NotificationTemplateContent> Contents { get; set; } = new List<NotificationTemplateContent>();
    }
}