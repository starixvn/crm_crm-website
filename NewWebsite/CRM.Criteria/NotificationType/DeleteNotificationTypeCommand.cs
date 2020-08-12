namespace CRM.Criteria.NotificationType
{
    using Fanex.Data.Repository;

    public class DeleteNotificationTypeCommand : NonQueryCommand
    {
       public int NotificationTypeId { get; set; }
        public int UserId { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        
        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationType_Del";

        public override bool IsValid() => 
            NotificationTypeId > 0
            && UserId > 0;
    }
}