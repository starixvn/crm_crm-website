namespace Fanex.CRM.BackgroundJob.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class ImportVoidingMessageForChatBotCriteria : CriteriaBase
    {
        public string MessageIds { get; set; }

        public override string GetSettingKey() => "Acc_CRM_ChatBot_crmMessageImport_Ins";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(MessageIds);
    }
}