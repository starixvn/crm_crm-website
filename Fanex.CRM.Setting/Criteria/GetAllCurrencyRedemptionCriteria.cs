// --------------------------------------------------------------------------
// <copyright file="GetAllCurrencyRedemptionCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.14">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All Currency Redemption Statistics Criteria.
    /// </summary>
    public class GetAllCurrencyRedemptionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Redemption_Statisics_GetCurrencyList";
        } 
    }
}
