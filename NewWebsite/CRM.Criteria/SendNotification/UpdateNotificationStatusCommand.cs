namespace CRM.Criteria.SendNotification
{
    using CRM.Core.Models;

    public class UpdateNotificationStatusCommand : BaseNonQueryCommand
    {
        public string SentStatus { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationStatus_Upd";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(SentStatus);
    }
}