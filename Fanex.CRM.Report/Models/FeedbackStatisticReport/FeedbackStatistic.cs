namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    [Serializable]
    public class FeedbackStatistic
    {
        public string Title { get; set; }

        public IList<BaseModel> Usernames { get; set; }

        public int TotalItems { get; set; }

        public IList<FeedbackCategory> FeedbackCategories { get; set; }

        public int[] GrandTotals { get; set; }
    }
}