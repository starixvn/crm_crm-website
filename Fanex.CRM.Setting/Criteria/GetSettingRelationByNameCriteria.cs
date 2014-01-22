// --------------------------------------------------------------------------
// <copyright file="GetSettingRelationByNameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Setting Relation By Name Criteria.
    /// </summary>
    public class GetSettingRelationByNameCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "settingname")]
        public string SettingName { get; set; }

        [SpParam(SqlParamName = "parentid")]
        public int ParentId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetRelationBySettingName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
