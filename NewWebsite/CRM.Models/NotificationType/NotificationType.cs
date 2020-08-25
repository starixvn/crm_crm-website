using System.Collections.Generic;

namespace CRM.Models.NotificationType
{
    public class NotificationType
    {
        public int NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public int SystemId { get; set; }

        public string SystemName { get; set; }

        public IEnumerable<RecipientType> RecipientTypes { get; set; }

        public IEnumerable<NotificationChatType> ChatTypes { get; set; }
    }
}