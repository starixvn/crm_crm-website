namespace CRM.Criteria.SendNotification
{
    using CRM.Core.Models;

    public class WriteLogMessageCommand : BaseNonQueryCommand
    {
        public int ReferenceId { get; set; }

        public int BotId { get; set; }

        public string LetstalkId { get; set; }

        public string Content { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_LogMessage_Ins";

        public override bool IsValid() => BotId > 0 &&
            ReferenceId > 0 &&
            !string.IsNullOrWhiteSpace(LetstalkId) &&
            !string.IsNullOrWhiteSpace(Content);
    }
}