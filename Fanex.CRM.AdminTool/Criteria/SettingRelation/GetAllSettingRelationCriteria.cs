// --------------------------------------------------------------------------
// <copyright file="GetAllSettingRelationCriteria.cs" company="">
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

    /// <summary>
    /// Get All Setting Relation Criteria.
    /// </summary>
    public class GetAllSettingRelationCriteria : BasedCriteria
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
