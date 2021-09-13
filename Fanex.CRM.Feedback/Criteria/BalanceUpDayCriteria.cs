namespace Fanex.CRM.Feedback.Criteria
{
    using Fanex.Data.Repository;

    public class BalanceUpDayCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_SelBalanceUpDay";
        }

        public override bool IsValid() => true;
    }
}
