using CRM.Core.Enumerables;

namespace CRM.Models.SendNotification
{
    public class NotifcationRecipient
    {
        public int NotificationId { get; set; }

        public int ConversationGroupId { get; set; }

        public int RecipientId { get; set; }

        public int ChatTypeId { get; set; }

        public int BotId { get; set; }

        public string LetstalkConversationId { get; set; }

        public ChatType ChatType
        {
            get
            {
                return (ChatType)ChatTypeId;
            }
        }
    }
}