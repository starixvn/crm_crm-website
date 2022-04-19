namespace Fanex.CRM.Dashboard.Criteria
{
    using Fanex.CRM.Common.Criteria;
    using Fanex.Data.Repository;
    public class GetCustomerNewRemarksCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerRemarkNewSel";
        }

        public override bool IsValid() => true;
    }
}
