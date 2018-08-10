namespace Fanex.CRM.Report.Models.SummaryReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SubCategorySummary
    {
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public IList<Summary> Summaries { get; set; }
    }
}
