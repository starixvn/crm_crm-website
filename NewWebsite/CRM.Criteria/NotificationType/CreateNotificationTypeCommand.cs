namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class CreateNotificationTypeCommand : NonQueryCommand
    {
        public int NotificationTypeId { get; set; }
        public string Name { get; set; }
        public int ChatType { get; set; }
        public int RecipientType { get; set; }
        public int System { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public override string GetSettingKey() => "";
        public override bool IsValid() =>
            NotificationTypeId > 0
            && !string.IsNullOrWhiteSpace(Name);
    }
}