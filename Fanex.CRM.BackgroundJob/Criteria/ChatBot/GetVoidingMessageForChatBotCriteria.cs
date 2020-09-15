namespace Fanex.CRM.BackgroundJob.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class GetVoidingMessageForChatBotCriteria : CriteriaBase
    {
        public string Messages { get; set; }

        public override string GetSettingKey() => "Acc_CRM_Chatbot_AbnormalBets_GetMessage";

        public override bool IsValid() => true;
    }
}