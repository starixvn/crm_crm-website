// --------------------------------------------------------------------------
// <copyright file="SubCategorySummary.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.SummaryReport
{
    using System.Collections.Generic;

    /// <summary>
    /// SubCategorySummary model.
    /// </summary>
    public class SubCategorySummary
    {
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public IList<Summary> Summaries { get; set; }
    }
}
