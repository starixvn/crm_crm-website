// --------------------------------------------------------------------------
// <copyright file="GetAllLicenseeTypeCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.11.19">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;

    /// <summary>
    /// Get all Licensee type criteria.
    /// </summary>
    public class GetAllLicenseeTypeCriteria : BasedCriteria
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllLicenseeType";
        }
    }
}
