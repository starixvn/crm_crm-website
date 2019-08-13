namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetAllSubCategoryCriteria : BasedLanguageCriteria
    {
        [SpParam(SqlParamName = "MainCategoryID")]
        public int ParentId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllSubCategory";
        }

        public override bool IsValid() => true;
    }
}