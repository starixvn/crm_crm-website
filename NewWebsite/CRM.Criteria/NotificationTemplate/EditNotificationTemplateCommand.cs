namespace CRM.Criteria.NotificationTemplate
{
    using CRM.Core.Models;
    using Fanex.Data;

    public class EditNotificationTemplateCommand : BaseNonQueryCommand
    {
        public int NotificationTemplateId { get; set; }
        public int NotificationTypeId { get; set; }
        public string NotificationTemplateName { get; set; }

        public string Title { get; set; }

        [SpParam(SqlParamName = "Contents")]
        public string ContentXml { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationTemplate_Upd";

        public override bool IsValid() =>
            UserId > 0
            && NotificationTemplateId > 0
            && NotificationTypeId > 0;
    }
}