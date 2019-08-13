namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class GetAllCurrencyCriteria : CriteriaBase
    {
        public GetAllCurrencyCriteria()
        {
            IsTest = true;
        }

        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCurrency";
        }

        public override bool IsValid() => true;
    }
}