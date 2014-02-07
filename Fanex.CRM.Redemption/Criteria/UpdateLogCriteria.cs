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
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update Log Criteria.
    /// </summary>
    public class UpdateLogCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "redemptionid")]
        public int RedemptionId { get; set; }

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
