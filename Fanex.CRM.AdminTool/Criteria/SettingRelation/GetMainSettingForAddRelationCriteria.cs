// --------------------------------------------------------------------------
// <copyright file="GetMainSettingForAddRelationCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using Fanex.CRM.Common.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Main Setting Criteria.
    /// </summary>
    public class GetMainSettingForAddRelationCriteria : CriteriaBase
    {
        public Setting MainSetting { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
