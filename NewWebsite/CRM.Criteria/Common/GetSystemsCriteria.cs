namespace CRM.Criteria.Common
{
    using Fanex.Data.Repository;

    public class GetSystemsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Acc_CRM_ChatBot_System_Sel";

        public override bool IsValid() => true;
    }
}