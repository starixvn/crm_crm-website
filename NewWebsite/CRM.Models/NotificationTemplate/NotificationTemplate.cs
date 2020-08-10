namespace CRM.Models.NotificationTemplate
{
    using System.Collections.Generic;

    public class NotificationTemplate
    {
        public int NotificationTemplateId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public IEnumerable<NotificationTemplateContent> Contents { get; set; } = new List<NotificationTemplateContent>();

        public int NotificationTypeId { get; set; }
        public string NotificationType { get; set; }
    }
}