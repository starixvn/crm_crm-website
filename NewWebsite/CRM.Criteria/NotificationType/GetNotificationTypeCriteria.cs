namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class GetNotificationTypeCriteria : CriteriaBase
    {
        public string RecipientTypeIds { get; set; }

        public string ChatTypeIds { get; set; }

        public string SystemId { get; set; }

        public string NotificationTypeName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Sel";

        public override bool IsValid() => true;
    }
}