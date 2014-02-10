// --------------------------------------------------------------------------
// <copyright file="UpdateLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Criteria
{
    using System;
    using System.Collections;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Log Criteria.
    /// </summary>
    public class UpdateLogCriteria : CriteriaBase
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

        [SpParam(SqlParamName = "userid")]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(SqlParamName = "ip")]
        public string IP
        {
            get
            {
                return Helper.GetClientIP();
            }
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("id", Id);
            param.Add("subject", StringExtension.GetValueNotNull(Subject));
            param.Add("claimtime", ClaimedTime);
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
            return "Redemption_Log_Update";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
