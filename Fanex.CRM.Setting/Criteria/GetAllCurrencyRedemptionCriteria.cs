namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllCurrencyRedemptionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Redemption_Statisics_GetCurrencyList";
        }

        public override bool IsValid() => true;
    }
}