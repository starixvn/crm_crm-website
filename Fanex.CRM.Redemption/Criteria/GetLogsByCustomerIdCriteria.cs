namespace Fanex.CRM.Redemption.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetLogsByCustomerIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Redemption_Log_GetInfoByCustomerId";
        }

        public override bool IsValid() => true;
    }
}