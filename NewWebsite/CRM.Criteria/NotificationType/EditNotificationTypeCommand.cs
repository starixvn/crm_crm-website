namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class EditNotificationTypeCommand : NonQueryCommand
    {
        public int NotificationTypeId { get; set; }
        public string Name { get; set; }
        public int ChatTypeId { get; set; }
        public int RecipientTypeId { get; set; }
        public int SystemId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() =>
            NotificationTypeId > 0
            && SystemId > 0
            && ChatTypeId > 0
            && RecipientTypeId > 0
            && !string.IsNullOrWhiteSpace(Name);
    }
}