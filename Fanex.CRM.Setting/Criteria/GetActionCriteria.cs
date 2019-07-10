// --------------------------------------------------------------------------
// <copyright file="GetActionCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.15">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.CRM.Setting.Constants;
    using System.Collections;

    /// <summary>
    /// GetActionCriteria class.
    /// </summary>
    public class GetActionCriteria : BasedCriteria
    {
        public int ActionType { get; set; }

        public override Hashtable GetSpParams()
        {
            if (ActionType == SettingConstants.ActivityLog)
            {
                var hastableParam = new Hashtable();
                hastableParam.Add("userrole", UserRole);

                return hastableParam;
            }

            return null;
        }

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
