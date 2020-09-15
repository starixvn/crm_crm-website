namespace CRM.Letstalk.Services.Criteria
{
    using Fanex.Data.Repository;

    public class GetNotificationToSendCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_SendNotification";

        public override bool IsValid() => true;
    }
}