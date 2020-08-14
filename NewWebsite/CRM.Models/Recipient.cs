namespace CRM.Models
{
    public class Recipient
    {
        public int NotificationId { get; set; }

        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}