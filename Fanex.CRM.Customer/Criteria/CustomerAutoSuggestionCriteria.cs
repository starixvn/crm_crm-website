// --------------------------------------------------------------------------
// <copyright file="CustomerAutoSuggestionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.28">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Customer auto suggestion criteria.
    /// </summary>
    public class CustomerAutoSuggestionCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public bool IsGuest { get; set; }

        public override string GetSettingKey()
        {
            return "autoSearchSuggest";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
