namespace Fanex.CRM.Report.Models.FeedbackStatisticReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class FeedbackSubcategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<int> Resolves { get; set; }

        public int Total
        {
            get
            {
                return Resolves == null || Resolves.Count == 0
                    ? 0
                    : Resolves.Sum();
            }
        }
    }
}
