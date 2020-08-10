using System;
using CRM.Core.Enumerables;

namespace CRM.Models.NotificationList
{
    public class GetGridDataParam
    {
        public string NotificationTypeIds { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string GroupHeadlineIds { get; set; }

        public string RecipientIds { get; set; }

        public int MessageId { get; set; }

        public int NotificationIdFrom { get; set; }

        public int NotificationIdTo { get; set; }

        public string Content { get; set; }

        public string Statuses { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}