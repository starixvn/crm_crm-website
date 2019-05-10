// --------------------------------------------------------------------------
// <copyright file="GetAllSubCategoryCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Sub Category Criteria.
    /// </summary>
    public class GetAllSubCategoryCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "MainCategoryID")]
        public int ParentId { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllSubCategory";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
