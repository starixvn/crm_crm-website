// --------------------------------------------------------------------------
// <copyright file="GetAllSettingRelationCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
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
