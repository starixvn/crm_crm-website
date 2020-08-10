namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class DeleteNotificationTypeCommand : NonQueryCommand
    {
        public int NotificationTypeId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        
        public override string GetSettingKey() => "";

        public override bool IsValid() => false;
    }
}