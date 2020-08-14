namespace CRM.Criteria.NotificationList
{
    using System;
    using Fanex.Data.Repository;

    public class GetNotificationListCriteria : CriteriaBase
    {
        public string NotificationTypeIds { get; set; }

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

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationList_Sel";

        public override bool IsValid() => !string.IsNullOrEmpty(NotificationTypeIds) && !string.IsNullOrEmpty(Status);
    }
}