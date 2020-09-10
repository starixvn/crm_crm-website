namespace CRM.Models.SendNotification
{
    using CRM.Core.Extensions.XML;

    [XMLName("Result")]
    public class SendNotificationResult
    {
        public int NotificationId { get; set; }

        public int RecipientId { get; set; }

        public int ConversationGroupId { get; set; }

        public string ErrMsg { get; set; }

        public int ErrCode { get; set; }

        public bool IsSuccess { get; set; }
    }
}