// --------------------------------------------------------------------------
// <copyright file="CustomerSuggestion.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.25">Created</change>
// </history>

namespace Fanex.CRM.Customer.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Customer Suggestion model.
    /// </summary>
    [Serializable]
    public class CustomerSuggestion
    {
        public string Query { get; set; }

        public IList<string> Suggestions { get; set; }
    }
}