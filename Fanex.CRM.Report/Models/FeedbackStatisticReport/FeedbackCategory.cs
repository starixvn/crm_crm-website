namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class FeedbackCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<FeedbackSubcategory> FeedbackSubcategories { get; set; }

        public FeedbackSubcategory SubTotal { get; set; }
    }
}
