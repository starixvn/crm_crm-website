namespace CRM.Criteria.NotificationList
{
    using Fanex.Data.Repository;

    public class EditNotificationListCommand : NonQueryCommand
    {
        public int NotificationId { get; set; }

        public string Remark { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationList_Upd";

        public override bool IsValid() => NotificationId > 0 && UserId > 0;
    }
}