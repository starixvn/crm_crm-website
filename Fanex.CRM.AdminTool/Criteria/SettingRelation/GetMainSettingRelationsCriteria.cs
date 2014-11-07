// --------------------------------------------------------------------------
// <copyright file="GetMainSettingRelationsCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get Main Setting Relation Criteria.
    /// </summary>
    public class GetMainSettingRelationsCriteria : CriteriaBase
    {
        public int ParentId { get; set; }

        public SettingInfo MainSetting { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingname", MainSetting.SettingType);
            param.Add("parentid", ParentId);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetRelationBySettingName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
