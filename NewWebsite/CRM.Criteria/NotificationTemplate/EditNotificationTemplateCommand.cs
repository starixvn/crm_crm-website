namespace CRM.Criteria.NotificationTemplate
{
    using Fanex.Data.Repository;

    public class EditNotificationTemplateCommand : NonQueryCommand
    {
        public int NotificationTemplateId { get; set; }
        public int NotificationTypeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ContentEN { get; set; }
        public string ContentSC { get; set; }
        public string ContentENSC { get; set; }
        public bool IsCombineContentENSC { get; set; }
        public int ModifierId { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() =>
            ModifierId > 0
            && NotificationTemplateId > 0
            && NotificationTypeId > 0
            && !string.IsNullOrWhiteSpace(Name);
    }
}