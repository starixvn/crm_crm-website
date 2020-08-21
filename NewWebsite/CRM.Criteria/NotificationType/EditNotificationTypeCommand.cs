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

        public override bool IsValid() =>
#pragma warning disable S1067 // Expressions should not be too complex
            NotificationTypeId > 0
            && SystemId > 0
            && UserId > 0
            && !string.IsNullOrWhiteSpace(ChatTypeIds)
            && !string.IsNullOrWhiteSpace(RecipientTypeIds)
            && !string.IsNullOrWhiteSpace(NotificationTypeName);
#pragma warning restore S1067 // Expressions should not be too complex
    }
}