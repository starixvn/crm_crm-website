namespace Fanex.CRM.Customer.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetUplineInfoOfUserCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_GetUplineList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}