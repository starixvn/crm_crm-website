namespace CRM.Models.NotificationType
{
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }

        public int ChatTypeId { get; set; }

        public int RecipientTypeId { get; set; }

        public int SystemId { get; set; }

        public string Name { get; set; }
    }
}