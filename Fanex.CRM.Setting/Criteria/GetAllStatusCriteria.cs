// --------------------------------------------------------------------------
// <copyright file="GetAllStatusCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get All Language Criteria.
    /// </summary>
    public class GetAllStatusCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "type")]
        public int ViewType { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
