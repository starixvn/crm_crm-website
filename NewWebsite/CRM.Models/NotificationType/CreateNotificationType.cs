namespace CRM.Models.NotificationType
{
    public class CreateNotificationType
    {
        public string NotificationTypeName { get; set; }

        public string ChatTypeIds { get; set; }

        public string RecipientTypeIds { get; set; }

        public int SystemId { get; set; }
    }
}