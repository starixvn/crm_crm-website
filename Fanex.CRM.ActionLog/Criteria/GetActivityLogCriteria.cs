// --------------------------------------------------------------------------
// <copyright file="GetActivityLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Criteria
{
    using Common.Enumerations;
    using Common.Services;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Activity Log criteria.
    /// </summary>
    public class GetActivityLogCriteria : GetActionLogCriteria
    {
        public GetActivityLogCriteria()
        {
            UserRole = AppSession.CurrentUser.UserRole;
        }

        public UserRole UserRole { get; set; }

        [SpParam(SqlParamName = "CustId")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_LogActionActivityList";
        }

        public override bool IsValid() => true;
    }
}