// --------------------------------------------------------------------------
// <copyright file="GetSettingLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
            return "";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
