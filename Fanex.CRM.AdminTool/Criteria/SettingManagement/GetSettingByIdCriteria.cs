// --------------------------------------------------------------------------
// <copyright file="GetSettingByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Criteria.SettingManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Setting By Identify Criteria.
    /// </summary>
    public class GetSettingByIdCriteria : CriteriaBase
    {
        public int SettingId { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}
