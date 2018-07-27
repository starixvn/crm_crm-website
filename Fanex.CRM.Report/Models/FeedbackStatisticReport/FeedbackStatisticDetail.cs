namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using Fanex.CRM.Common.Models;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class FeedbackStatisticDetail
    {
        public string Title { get; set; }

        public int TotalItems { get; set; }

        public IList<FeedbackDetail> Feedbacks { get; set; }
    }
}
