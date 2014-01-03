// --------------------------------------------------------------------------
// <copyright file="CustomersSuggestion.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities.ViewModel
{
    using System.Collections.Specialized;

    /// <summary>
    /// Customers Suggestion.
    /// </summary>
    public class CustomersSuggestion
    {
        public string[] QueryData { get; set; }

        public string QueryParameters { get; set; }

        public StringCollection Suggestions { get; set; }
    }
}
