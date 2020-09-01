namespace CRM.Criteria.NotificationType
{
    using CRM.Core.Models;

    public class CreateNotificationTypeCommand : BaseNonQueryCommand
    {
        public string NotificationTypeName { get; set; }
        public string ChatTypeIds { get; set; }
        public string RecipientTypeIds { get; set; }
        public int SystemId { get; set; }
        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Ins";

        public override bool IsValid() =>
             UserId > 0
             && SystemId > 0;
    }
}