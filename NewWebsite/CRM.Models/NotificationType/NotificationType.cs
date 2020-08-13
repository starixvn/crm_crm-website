namespace CRM.Models.NotificationType
{
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public int ChatType { get; set; }

        public int RecipientType { get; set; }

        public int SystemName { get; set; }
    }
}