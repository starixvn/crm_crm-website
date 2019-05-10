namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class SubProductCriteria : BasedCriteria
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
