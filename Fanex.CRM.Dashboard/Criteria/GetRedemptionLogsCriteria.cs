namespace Fanex.CRM.Dashboard.Criteria
{
    using Fanex.Data.Repository;

    public class GetRedemptionLogsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_GetRedemptionLogs";
        }

        public override bool IsValid() => true;
    }
}