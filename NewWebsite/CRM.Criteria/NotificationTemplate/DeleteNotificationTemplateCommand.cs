namespace CRM.Criteria.NotificationTemplate
{
    using Fanex.Data.Repository;

    public class DeleteNotificationTemplateCommand : NonQueryCommand
    {
        public int NotificationTemplateId { get; set; }
        public int UserId { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationTemplate_Del";

        public override bool IsValid() =>
            UserId > 0
            && NotificationTemplateId > 0;
    }
}