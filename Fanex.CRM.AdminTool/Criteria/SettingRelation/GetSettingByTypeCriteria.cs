// --------------------------------------------------------------------------
// <copyright file="GetSettingByTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using System.Collections;
    using Fanex.CRM.Common.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Setting By Type Criteria.
    /// </summary>
    public class GetSettingByTypeCriteria : CriteriaBase
    {
        public Setting MainSetting { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
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
