// --------------------------------------------------------------------------
// <copyright file="GetAllSiteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.CRM.Setting.Constants;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Site Criteria.
    /// </summary>
    public class GetAllSiteCriteria : CriteriaBase
    {
        public GetAllSiteCriteria()
        {
            LicenseeType = LicenseeTypeConstants.All;
        }

        public int LicenseeType { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllSite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
