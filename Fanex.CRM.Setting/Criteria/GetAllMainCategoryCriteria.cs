// --------------------------------------------------------------------------
// <copyright file="GetAllMainCategoryCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    /// <summary>
    /// Get All Main Category Criteria.
    /// </summary>
    public class GetAllMainCategoryCriteria : BasedCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllMainCategory";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
