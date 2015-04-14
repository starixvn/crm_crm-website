// --------------------------------------------------------------------------
// <copyright file="GetAllCurrencyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
            IsTest = 1;
        }

        /// <summary>
        /// Gets or sets the value indicating whether data will remove test currencies or not.
        /// </summary>
        /// <value>Default is 1, value = 0 will remove test currencies.</value>
        public int IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllCurrency";
        }
    }
}
