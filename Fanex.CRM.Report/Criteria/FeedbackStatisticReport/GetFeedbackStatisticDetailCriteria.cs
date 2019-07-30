namespace Fanex.CRM.Report.Criteria.FeedbackStatisticReport
{
    using System;
    using System.Collections;
    using Common.Criteria;
    using Fanex.Data.Repository;

    public class GetFeedbackStatisticDetailCriteria : BasedCriteria
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDuration { get; set; }

        public string ToDuration { get; set; }

        public int SelectionType { get; set; }

        public int MainCategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public int ValueId { get; set; }

        [SpParam(Ignored = true)]
        public bool FromCache { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize { get; set; }

        [SpParam(Ignored = true)]
        public bool ExportExcel { get; set; }

        public override string GetSettingKey()
        {
            return "Rpt_FeedbackStatisticDetails";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable
            {
                { "type", SelectionType },
                { "maincategoryid", MainCategoryId },
                { "subcategoryid", SubCategoryId },
                { "valueid", ValueId },
                { "fromdate", FromDate },
                { "todate", ToDate },
                { "fromduration", FromDuration ?? string.Empty },
                { "toduration", ToDuration ?? string.Empty },
                { "userrole", UserRole }
            };
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && (ToDate - FromDate).Days <= 365;
        }
    }
}