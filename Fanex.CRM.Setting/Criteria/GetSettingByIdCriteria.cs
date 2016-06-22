// --------------------------------------------------------------------------
// <copyright file="GetSettingByIdCriteria.cs" company="">
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
    /// Get Setting By Identify Criteria.
    /// </summary>
    public class GetSettingByIdCriteria : CriteriaBase
    {
        public int SettingId { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
