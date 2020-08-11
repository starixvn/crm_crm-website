namespace CRM.Criteria.NotificationTemplate
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class EditNotificationTemplateCommand : NonQueryCommand
    {
        public int NotificationTemplateId { get; set; }
        public int NotificationTypeId { get; set; }

        [SpParam(SqlParamName = "NotificationTemplateName")]
        public string Name { get; set; }

        public string Title { get; set; }

        [SpParam(SqlParamName = "Contents")]
        public string ContentXml { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationTemplate_Upd";

        public override bool IsValid() =>
#pragma warning disable S1067 // Expressions should not be too complex
            UserId > 0
            && NotificationTemplateId > 0
            && NotificationTypeId > 0
            && !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Title)
            && !string.IsNullOrWhiteSpace(ContentXml);
#pragma warning restore S1067 // Expressions should not be too complex
    }
}