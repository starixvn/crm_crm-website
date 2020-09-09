namespace CRM.Models.SendNotification
{
    using CRM.Core.Enumerables;

    public class NotificationMessage
    {
        public int NotifcationId { get; set; }

        public string Content { get; set; }

        public int ConversationGroupId { get; set; }

        public int RecipientId { get; set; }

        public int ChatTypeId { get; set; }

        public int BotId { get; set; }

        public string LetstalkConversationId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public ChatType ChatType
        {
            get
            {
                return (ChatType)ChatTypeId;
            }
        }
    }
}