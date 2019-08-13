namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.CRM.Setting.Constants;

    public class GetAllSiteCriteria : BasedLanguageCriteria
    {
        public GetAllSiteCriteria()
        {
            LicenseeType = LicenseeTypeConstants.All;
        }

        public int LicenseeType { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllSite";
        }

        public override bool IsValid() => true;
    }
}