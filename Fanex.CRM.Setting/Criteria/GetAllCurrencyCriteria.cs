namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllCurrencyCriteria : CriteriaBase
    {
        public GetAllCurrencyCriteria()
        {
            IsTest = true;
            Active = 1;
            OfferSite = 1;
        }

        public bool IsTest { get; set; }

        public int Active { get; set; }

        public int OfferSite { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCurrency";
        }

        public override bool IsValid() => true;
    }
}