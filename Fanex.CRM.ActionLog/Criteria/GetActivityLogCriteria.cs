// --------------------------------------------------------------------------
// <copyright file="GetActivityLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Activity Log criteria.
    /// </summary>
    public class GetActivityLogCriteria : GetActionLogCriteria
    {
        [SpParam(SqlParamName = "CustId")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_LogActionActivityList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
