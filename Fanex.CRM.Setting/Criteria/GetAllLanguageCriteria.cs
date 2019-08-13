namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllLanguageCriteria : BasedLanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllLanguage";
        }

        public override bool IsValid() => true;
    }
}