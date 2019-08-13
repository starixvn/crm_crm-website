namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetAllStatusCriteria : BasedLanguageCriteria
    {
        public GetAllStatusCriteria(int statusType)
        {
            ViewType = statusType;
        }

        [SpParam(SqlParamName = "type")]
        public int ViewType { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllStatus";
        }

        public override bool IsValid() => true;
    }
}