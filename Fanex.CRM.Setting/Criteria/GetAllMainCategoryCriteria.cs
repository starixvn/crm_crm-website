namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllMainCategoryCriteria : BasedLanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllMainCategory";
        }

        public override bool IsValid() => true;
    }
}