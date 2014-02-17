// --------------------------------------------------------------------------
// <copyright file="GetAllLanguageCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get All Language Criteria.
    /// </summary>
    public class GetAllLanguageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllLanguage";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
