// --------------------------------------------------------------------------
// <copyright file="CustomerSuggestionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.25">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Customer Suggestion Criteria.
    /// </summary>
    public class CustomerSuggestionCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        [SpParam(SqlParamName = "Username")]
        public string Query { get; set; }

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