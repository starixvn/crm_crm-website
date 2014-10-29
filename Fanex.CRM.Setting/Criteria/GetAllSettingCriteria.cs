// --------------------------------------------------------------------------
// <copyright file="GetAllSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Setting Criteria.
    /// </summary>
    public class GetAllSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
