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
    using Fanex.Data.Repository;

    /// <summary>
    /// GetActionCriteria class.
    /// </summary>
    public class GetActionCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "Type")]
        public int ActionType { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Stored procedure name.</returns>
        public override string GetSettingKey()
        {
            var settingKey = string.Empty;
            switch (ActionType)
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
