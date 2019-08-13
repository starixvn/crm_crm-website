namespace Fanex.CRM.Dashboard.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetCommunicationLogsCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_GetCommunicationLogs";
        }

        public override bool IsValid() => true;
    }
}