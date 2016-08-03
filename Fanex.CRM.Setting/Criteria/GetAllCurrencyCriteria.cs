// --------------------------------------------------------------------------
// <copyright file="GetAllCurrencyCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.11.21">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all currency criteria.
    /// </summary>
    public class GetAllCurrencyCriteria : CriteriaBase
    {
        public GetAllCurrencyCriteria()
        {
            IsTest = true;
        }

        /// <summary>
        /// Gets or sets the value indicating whether data will remove test currencies or not.
        /// </summary>
        /// <value>Default is 1, value = 0 will remove test currencies.</value>
        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCurrency";
        }
    }
}
