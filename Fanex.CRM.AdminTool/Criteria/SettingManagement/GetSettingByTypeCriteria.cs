// --------------------------------------------------------------------------
// <copyright file="GetSettingByTypeCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using Common.Criteria;
    using Fanex.CRM.Common.Models;
    using System.Collections;

    /// <summary>
    /// Get Setting By Type Criteria.
    /// </summary>
    public class GetSettingByTypeCriteria : BasedCriteria
    {
        public Setting MainSetting { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("UserRole", UserRole);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Setting_FeedbackTemplateSubject_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
