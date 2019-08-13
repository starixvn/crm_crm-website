namespace Fanex.CRM.Remark.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetRemarkCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CustomerRemarkSel";
        }

        public override bool IsValid() => true;
    }
}