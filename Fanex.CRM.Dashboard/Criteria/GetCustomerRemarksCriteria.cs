namespace Fanex.CRM.Dashboard.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetCustomerRemarksCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_GetCustomerRemarks";
        }

        public override bool IsValid() => true;
    }
}