// --------------------------------------------------------------------------
// <copyright file="GetDefaultSettingsCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.30">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Default Settings Criteria.
    /// </summary>
    public class GetDefaultSettingsCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetDefaultSettings";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
