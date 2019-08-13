namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllLicenseeTypeCriteria : BasedCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllLicenseeType";
        }

        public override bool IsValid() => true;
    }
}