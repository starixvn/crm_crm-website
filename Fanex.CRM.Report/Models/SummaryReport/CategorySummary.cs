// --------------------------------------------------------------------------
// <copyright file="CategorySummary.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.SummaryReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// CategorySummary model.
    /// </summary>
    [Serializable]
    public class CategorySummary
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<Summary> Summaries { get; set; }

        public IList<CategorySummary> SubCategories { get; set; }
    }
}
