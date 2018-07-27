namespace Fanex.CRM.Report.Criteria.FeedbackStatisticReport
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections;

    public class GetFeedbackStatisticCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDuration { get; set; }

        public string ToDuration { get; set; }

        public int SelectionType { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool ExportExcel { get; set; }

        public override string GetSettingKey()
        {
            return "FeedbackStatistics";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable
            {
                { "type", SelectionType },
                { "fromdate", FromDate },
                { "todate", ToDate },
                { "fromduration", FromDuration == null ? string.Empty : FromDuration },
                { "toduration", ToDuration == null ? string.Empty : ToDuration }
            };
        }
    }
}
