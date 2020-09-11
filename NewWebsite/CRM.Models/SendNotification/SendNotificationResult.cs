namespace CRM.Models.SendNotification
{
    using CRM.Core.Extensions.XML;

    [XMLName("r")]
    public class SendNotificationResult
    {
        public int NotificationId { get; set; }

        public int ConversationGroupId { get; set; }

        public string Message { get; set; }

        public string Status { get; set; }
    }
}