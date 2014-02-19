// --------------------------------------------------------------------------
// <copyright file="GetActionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.15">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.CRM.Setting.Constants;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetActionCriteria class.
    /// </summary>
    public class GetActionCriteria : CriteriaBase
    {
        public int Type { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Stored procedure name.</returns>
        public override string GetSettingKey()
        {
            var settingKey = string.Empty;
            switch (Type)
            {
                case SettingConstants.ActivityLog:
                    settingKey = "Acc_CRM_ActivityLogList";
                    break;

                case SettingConstants.SettingLog:
                    settingKey = "Acc_CRM_SettingLogList";
                    break;

                default:
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
