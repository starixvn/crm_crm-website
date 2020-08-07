namespace CRM.Models.NotificationType
{
    public class CreateNotificationType
    {
        public string Name { get; set; }
        public int ChatType { get; set; }

        public int RecipientType { get; set; }

        public int System { get; set; }
    }
}