namespace CRM.Models.NotificationType
{
    public class CreateNotificationType
    {
        public string Name { get; set; }
        public int ChatTypeId { get; set; }

        public int RecipientTypeId { get; set; }

        public int SystemId { get; set; }
    }
}