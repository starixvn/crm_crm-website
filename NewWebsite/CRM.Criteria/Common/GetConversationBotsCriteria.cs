namespace CRM.Criteria.Common
{
    using Fanex.Data.Repository;

    public class GetConversationBotsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_Bot_Sel";

        public override bool IsValid() => true;
    }
}