namespace CRM.Models.SendNotification
{
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