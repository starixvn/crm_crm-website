namespace CRM.Criteria.SendNotification
{
    using Fanex.Data.Repository;

    public class WriteLogMessageCommand : NonQueryCommand
    {
        public int NotificationId { get; set; }

        public int BotId { get; set; }

        public string ConversationId { get; set; }

        public string Content { get; set; }

        public bool IsBotBlocked { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_NotificationMsgLog_Ins";

        public override bool IsValid() => BotId > 0 &&
            NotificationId > 0 &&
            !string.IsNullOrWhiteSpace(ConversationId) &&
            !string.IsNullOrWhiteSpace(Content);
    }
}