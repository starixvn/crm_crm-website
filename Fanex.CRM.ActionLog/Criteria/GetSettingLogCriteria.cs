// --------------------------------------------------------------------------
// <copyright file="GetSettingLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Criteria
{
    /// <summary>
    /// Get Setting Log criteria.
    /// </summary>
    public class GetSettingLogCriteria : GetActionLogCriteria
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_LogActionSettingList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
