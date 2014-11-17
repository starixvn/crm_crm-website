// --------------------------------------------------------------------------
// <copyright file="GetAllCurrencyInformHeadlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.14">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Currency Inform Headline Criteria.
    /// </summary>
    public class GetAllCurrencyInformHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_CurrencyList";
        } 
    }
}
