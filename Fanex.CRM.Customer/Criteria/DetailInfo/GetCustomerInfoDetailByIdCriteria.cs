namespace Fanex.CRM.Customer.Criteria.DetailInfo
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetCustomerInfoDetailByIdCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public string CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfoDetail_GetInfoById_New";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}