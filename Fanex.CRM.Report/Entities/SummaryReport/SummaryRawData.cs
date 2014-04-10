// --------------------------------------------------------------------------
// <copyright file="SummaryRawData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.SummaryReport
{
    /// <summary>
    /// SummaryRawData class.
    /// </summary>
    public class SummaryRawData
    {
        public int M { get; set; }

        public string Month { get; set; }

        public int MainCategoryId { get; set; }

        public string MainCategory { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategory { get; set; }

        public int NoOfIssues { get; set; }
    }
}
