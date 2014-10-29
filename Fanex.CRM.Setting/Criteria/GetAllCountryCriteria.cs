// --------------------------------------------------------------------------
// <copyright file="GetAllCountryCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get All Country Criteria.
    /// </summary>
    public class GetAllCountryCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCountry";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
