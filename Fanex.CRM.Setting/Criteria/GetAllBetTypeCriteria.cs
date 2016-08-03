// --------------------------------------------------------------------------
// <copyright file="GetAllBetTypeCriteria.cs" company="">
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
    /// Get All Sub Category Criteria.
    /// </summary>
    public class GetAllBetTypeCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ProductID")]
        public int ParentId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllBetType";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
