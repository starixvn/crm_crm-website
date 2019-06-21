// --------------------------------------------------------------------------
// <copyright file="GetSettingByIdCriteria.cs" company="">
// Copyright (c) . All rights reserved.
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
    public class GetSettingCN88ByIdCriteria : CriteriaBase
    {
        public int SettingId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Acc_CRM_CN88_SettingSelBySettingID";
        }
    }
}
