namespace Fanex.CRM.Communication.Criteria
{
    using System;
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetLogsByCustomerIdCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "handler")]
        public string Handler { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetInfoByCustomerId";
        }

        public override bool IsValid() => true;
    }
}