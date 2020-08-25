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
#pragma warning disable S1067 // Expressions should not be too complex
             UserId > 0
             && SystemId > 0
            && !string.IsNullOrWhiteSpace(ChatTypeIds)
            && !string.IsNullOrWhiteSpace(RecipientTypeIds)
            && !string.IsNullOrWhiteSpace(NotificationTypeName);
#pragma warning restore S1067 // Expressions should not be too complex
    }
}