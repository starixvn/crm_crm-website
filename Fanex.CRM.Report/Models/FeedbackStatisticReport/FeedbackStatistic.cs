namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using Fanex.CRM.Infrastructure.Models;
    using System;
    using System.Collections.Generic;

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
