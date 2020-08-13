namespace CRM.Models.NotificationType
{
    using System;
    using CRM.Core.Extensions.XML;

    [Serializable]
    [XMLName("Row")]
    public class NotificationTypeRecipient
    {
        public int RecipientId { get; set; }
        public string Name { get; set; }
    }
}