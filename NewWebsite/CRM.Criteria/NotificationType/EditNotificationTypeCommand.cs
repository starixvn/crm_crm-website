namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class EditNotificationTypeCommand : NonQueryCommand
    {
        public int UserId { get; set; }
        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public string ChatTypeIds { get; set; }
        public string RecipientTypeIds { get; set; }
        public int SystemId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Upd";

        public override bool IsValid() =>
            NotificationTypeId > 0
            && SystemId > 0
            && UserId > 0
            && !string.IsNullOrWhiteSpace(NotificationTypeName);
    }
}