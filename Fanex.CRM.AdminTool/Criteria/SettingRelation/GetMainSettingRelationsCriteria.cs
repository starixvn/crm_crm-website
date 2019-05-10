// --------------------------------------------------------------------------
// <copyright file="GetMainSettingRelationsCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using Common.Criteria;
    using Fanex.CRM.Common.Models;
    using System.Collections;

    /// <summary>
    /// Get Main Setting Relation Criteria.
    /// </summary>
    public class GetMainSettingRelationsCriteria : BasedCriteria
    {
        public int ParentId { get; set; }

        public Setting MainSetting { get; set; }

        /// <summary>
        /// Gets the SP parameters.
        /// </summary>
        /// <returns>The SP parameters.</returns>
        public override Hashtable GetSpParams()
        {
            var param = new Hashtable();
            param.Add("settingname", MainSetting.SettingType);
            param.Add("parentid", ParentId);
            param.Add("UserRole", UserRole);
            return param;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingGetRelationBySettingName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
