namespace CRM.Criteria.Common
{
    using Fanex.Data.Repository;

    public class GetRecipientTypesCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_RecipientType_Sel";

        public override bool IsValid() => true;
    }
}