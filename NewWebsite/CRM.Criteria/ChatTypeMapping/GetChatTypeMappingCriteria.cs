namespace CRM.Criteria.ChatTypeMapping
{
    using Fanex.Data.Repository;

    public class GetChatTypeMappingCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_ChatTypeMapping_Sel";

        public override bool IsValid() => true;
    }
}