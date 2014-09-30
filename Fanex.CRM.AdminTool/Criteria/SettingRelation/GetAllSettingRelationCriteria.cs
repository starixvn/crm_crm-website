// --------------------------------------------------------------------------
// <copyright file="GetAllSettingRelationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingRelation
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Setting Relation Criteria.
    /// </summary>
    public class GetAllSettingRelationCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingRelationSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
