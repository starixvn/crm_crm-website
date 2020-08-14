namespace CRM.Criteria.NotificationTemplate
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetNotificationTemplateCriteria : CriteriaBase
    {
        public int? NotificationTemplateId { get; set; }
        public int? NotificationTypeId { get; set; }
        public string NotificationTemplateName { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationTemplate_Sel";

        public override bool IsValid() => true;
    }
}