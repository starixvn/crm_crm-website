namespace CRM.Criteria.ChatBot
{
    using Fanex.Data.Repository;

    public class GetChatTypesCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChatType_Sel";

        public override bool IsValid() => true;
    }
}