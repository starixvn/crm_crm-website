using System;
using CRM.Core.Enumerables;

namespace CRM.Models.NotificationList
{
    public class GetGridDataParam
    {
        public int NotificationTypeId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int GroupHeadline { get; set; }

        public int Recipient { get; set; }

        public int MessageId { get; set; }

        public int NotificationIdFrom { get; set; }

        public int NotificationIdTo { get; set; }

        public string Content { get; set; }

        public Status Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}