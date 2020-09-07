namespace CRM.Criteria.NotificationType
{
    using CRM.Core.Models;

    public class EditNotificationTypeCommand : BaseNonQueryCommand
    {
        public int UserId { get; set; }
        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public string ChatTypeIds { get; set; }
        public string RecipientTypeIds { get; set; }
        public int SystemId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Upd";

        private bool IsValidString() => !string.IsNullOrWhiteSpace(NotificationTypeName) &&
            !string.IsNullOrWhiteSpace(ChatTypeIds) &&
            !string.IsNullOrWhiteSpace(RecipientTypeIds);

        public override bool IsValid() => 
            IsValidString() &&
            NotificationTypeId > 0
            && SystemId > 0
            && UserId > 0;
    }
}