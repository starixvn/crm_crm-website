namespace Fanex.CRM.BackgroundJob.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class GetChatBotSystemSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_Chatbot_SystemSetting_Sel";

        public override bool IsValid() => true;
    }
}