namespace CRM.Criteria.NotificationList
{
    using Fanex.Data.Repository;

    public class ResendNotificationsCommand : NonQueryCommand
    {
        public int UserId { get; set; }

        public string NotificationIds { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_Chatbot_ResendNotification";

        public override bool IsValid() => !string.IsNullOrEmpty(NotificationIds) && UserId > 0;
    }
}