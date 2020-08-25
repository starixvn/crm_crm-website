namespace CRM.Criteria.NotificationType
{
    using CRM.Core.Models;

    public class DeleteNotificationTypeCommand : BaseNonQueryCommand
    {
        public int NotificationTypeId { get; set; }
        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Del";

        public override bool IsValid() =>
            NotificationTypeId > 0
            && UserId > 0;
    }
}