namespace CRM.Letstalk.Services.Models.SendNotification
{
    using CRM.Core.Enumerables;

    public class NotificationMessage
    {
        public int NotificationId { get; set; }

        public string Content { get; set; }

        public int ConversationGroupId { get; set; }

        public int ChatTypeId { get; set; }

        public int BotId { get; set; }

        public string ConversationId { get; set; }

        public ChatType ChatType
        {
            get
            {
                return (ChatType)ChatTypeId;
            }
        }
    }
}