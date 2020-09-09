namespace CRM.Criteria.SendNotification
{
    using Fanex.Data.Repository;

    public class GetNotificationToSendCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationToSend_Sel";

        public override bool IsValid() => true;
    }
}