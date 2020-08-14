namespace CRM.Models.NotificationType
{
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public string ChatType { get; set; }

        public string RecipientType { get; set; }

        public string SystemName { get; set; }
    }
}