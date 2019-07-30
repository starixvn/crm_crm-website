namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    public class GetAllCommunicationToolCriteria : BasedLanguageCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCommunicationTool";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}