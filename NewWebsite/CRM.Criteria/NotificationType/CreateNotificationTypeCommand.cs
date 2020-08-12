namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class CreateNotificationTypeCommand : NonQueryCommand
    {
        public string NotificationTypeName { get; set; }
        public string ChatTypeId { get; set; }
        public string RecipientTypeId { get; set; }
        public int SystemId { get; set; }
        public int UserId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Ins";
        public override bool IsValid() =>
             UserId > 0
            && !string.IsNullOrWhiteSpace(NotificationTypeName);
    }
}