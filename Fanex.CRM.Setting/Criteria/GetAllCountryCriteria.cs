namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllCountryCriteria : BasedLanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCountry";
        }

        public override bool IsValid() => true;
    }
}