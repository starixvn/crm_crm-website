namespace CRM.Models.NotificationType
{
    public class CreateNotificationType
    {
        public string NotificationTypeName { get; set; }

        public string ChatTypeId{ get; set; }

        public string RecipientTypeId { get; set; }

        public int SystemId { get; set; }
    }
}