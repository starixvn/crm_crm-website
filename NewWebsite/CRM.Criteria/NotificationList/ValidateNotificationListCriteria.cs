namespace CRM.Criteria.NotificationList
{
    using Fanex.Data.Repository;

    public class ValidateNotificationListCriteria : CriteriaBase
    {
        public string NotificationIds { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationListValidation";

        public override bool IsValid() => !string.IsNullOrEmpty(NotificationIds);
    }
}