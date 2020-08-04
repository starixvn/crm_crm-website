namespace CRM.Models.NotificationType
{
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }

        public int ChatType { get; set; }

        public int RecipientType { get; set; }

        public int System { get; set; }

        public string Name { get; set; }
    }
}