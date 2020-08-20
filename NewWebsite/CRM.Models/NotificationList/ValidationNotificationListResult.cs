namespace CRM.Models.NotificationList
{
    using System.Collections.Generic;

    public class ValidationNotificationListResult
    {
        public bool IsSingle { get; set; }

        public IEnumerable<int> InActiveNotificationIds { get; set; }

        public IEnumerable<NotificationListResend> ResendDataList { get; set; }
    }
}