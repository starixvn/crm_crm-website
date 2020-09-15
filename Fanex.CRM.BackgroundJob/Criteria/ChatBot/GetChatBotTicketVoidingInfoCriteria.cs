namespace Fanex.CRM.BackgroundJob.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class GetChatBotTicketVoidingInfoCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_Chatbot_AbnormalBets_VoidingInfo_Sel";

        public override bool IsValid() => true;
    }
}