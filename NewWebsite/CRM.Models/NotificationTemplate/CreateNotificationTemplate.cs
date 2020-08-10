namespace CRM.Models.NotificationTemplate
{
    using System.Collections.Generic;

    public class CreateNotificationTemplate
    {
        public int NotificationTypeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public IEnumerable<NotificationTemplateContent> Contents { get; set; } = new List<NotificationTemplateContent>();
    }
}