using System;

namespace CRM.Models.NotificationList
{
    public class GetGridDataParam
    {
        public string NotificationTypeId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string Headline { get; set; }

        public string RecipientName { get; set; }

        public int? MessageId { get; set; }

        public int? NotificationId { get; set; }

        public int? NotificationIdLower { get; set; }

        public int? NotificationIdUpper { get; set; }

        public string Content { get; set; }

        public string Status { get; set; }

        public int TotalRows { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}