namespace CRM.Criteria.NotificationTemplate
{
    using Fanex.Data.Repository;

    public class EditNotificationTemplateCommand : NonQueryCommand
    {
        public int NotificationTemplateId { get; set; }
        public int NotificationTypeId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ContentXml { get; set; }
        public int ModifierId { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "";

        public override bool IsValid() =>
#pragma warning disable S1067 // Expressions should not be too complex
            ModifierId > 0
            && NotificationTemplateId > 0
            && NotificationTypeId > 0
            && !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Title)
            && !string.IsNullOrWhiteSpace(ContentXml);
#pragma warning restore S1067 // Expressions should not be too complex
    }
}