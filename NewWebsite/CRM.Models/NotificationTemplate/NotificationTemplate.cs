namespace CRM.Models.NotificationTemplate
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class NotificationTemplate
    {
        public int NotificationTemplateId { get; set; }
        public string NotificationTemplateName { get; set; }
        public string Title { get; set; }

        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }

        public IEnumerable<NotificationTemplateContent> Contents { get; set; } = new List<NotificationTemplateContent>();
    }
}