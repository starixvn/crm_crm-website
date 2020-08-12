namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class EditNotificationTypeCommand : NonQueryCommand
    {
        public int NotificationTypeId { get; set; }
        public string NotificationTypeName { get; set; }
        public string ChatTypeId { get; set; }
        public string RecipientTypeId { get; set; }
        public int SystemId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Upd";

        public override bool IsValid() =>
            NotificationTypeId > 0
            && SystemId > 0
            && !string.IsNullOrWhiteSpace(NotificationTypeName);
    }
}