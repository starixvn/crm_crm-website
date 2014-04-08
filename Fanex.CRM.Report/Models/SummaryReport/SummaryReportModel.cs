// --------------------------------------------------------------------------
// <copyright file="SummaryReportModel.cs" company="Nexcel Solutions Vietnam">
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
    /// SummaryReportModel model.
    /// </summary>
    public class SummaryReportModel
    {
        public string ReportTitle { get; set; }

        public IList<CategorySummary> CategorySummaries { get; set; }

        public SubCategorySummary GrandTotal { get; set; }
    }
}
