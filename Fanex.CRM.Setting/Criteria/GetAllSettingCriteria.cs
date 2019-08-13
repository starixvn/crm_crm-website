namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllSettingCriteria : BasedLanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingSel";
        }

        public override bool IsValid() => true;
    }
}