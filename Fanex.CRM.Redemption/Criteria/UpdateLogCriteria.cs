namespace Fanex.CRM.Redemption.Criteria
{
    using System;
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Extensions;
    using Fanex.Data.Repository;

    public class UpdateLogCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "Id")]
        public int Id { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "claimtime")]
        public DateTime ClaimedTime { get; set; }

        [SpParam(SqlParamName = "remark")]
        public string Remark { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        [SpParam(SqlParamName = "currencyid")]
        public int CurrencyId { get; set; }

        [SpParam(SqlParamName = "doneby")]
        public string DoneBy { get; set; }

        [SpParam(SqlParamName = "amount")]
        public decimal Amount { get; set; }

        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("id", Id);
            param.Add("subject", StringExtension.GetValueNotNull(Subject));
            param.Add("claimtime", ClaimedTime);
            param.Add("remark", StringExtension.GetValueNotNull(Remark));
            param.Add("statusid", StatusId);
            param.Add("currencyid", CurrencyId);
            param.Add("doneby", DoneBy);
            param.Add("amount", Amount);

            param.Add("userid", UserId);
            param.Add("ip", IP);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Redemption_Log_Update";
        }

        public override bool IsValid() => true;
    }
}