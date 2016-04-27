namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    public class SubProductCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ProductID")]
        public int ParentId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllSubProduct";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
