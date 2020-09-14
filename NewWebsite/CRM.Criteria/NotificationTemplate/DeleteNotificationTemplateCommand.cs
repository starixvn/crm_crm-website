namespace CRM.Criteria.NotificationTemplate
{
    using CRM.Core.Models;

    public class DeleteNotificationTemplateCommand : BaseNonQueryCommand
    {
        public int NotificationTemplateId { get; set; }
        public string NotificationTemplateName { get; set; }
        public int UserId { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationTemplate_Del";

        public override bool IsValid() =>
            UserId > 0
            && NotificationTemplateId > 0;
    }
}