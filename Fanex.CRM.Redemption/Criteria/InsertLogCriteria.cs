// --------------------------------------------------------------------------
// <copyright file="InsertLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Criteria
{
    using System;
    using System.Collections;
    using Fanex.CRM.Common.Criteria;
    using Fanex.CRM.Common.Extensions;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert Log Criteria.
    /// </summary>
    public class InsertLogCriteria : UpdateCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "subject")]
        public string Subject { get; set; }

        [SpParam(SqlParamName = "claimtime")]
        public DateTime ClaimedTime { get; set; }

        public int CurrencyId { get; set; }

        public string DoneBy { get; set; }

        public decimal Amount { get; set; }

        [SpParam(SqlParamName = "remark")]
        public string Remark { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("custid", CustomerId);
            param.Add("subject", StringExtension.GetValueNotNull(Subject));
            param.Add("claimtime", ClaimedTime);
            param.Add("currencyid", CurrencyId);
            param.Add("doneby", DoneBy);
            param.Add("amount", Amount);
            param.Add("remark", StringExtension.GetValueNotNull(Remark));
            param.Add("statusid", StatusId);

            param.Add("userid", UserId);
            param.Add("ip", IP);

            param.Add("errcode", ErrorCode);
            param.Add("errmsg", ErrorMessage);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Redemption_Log_Insert";
        }

        public override bool IsValid() => true;
    }
}