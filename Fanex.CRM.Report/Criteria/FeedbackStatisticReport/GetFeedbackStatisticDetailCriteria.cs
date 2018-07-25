// --------------------------------------------------------------------------
// <copyright file="GetFeedbackStatisticDetailCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.15">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.FeedbackStatisticReport
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections;

    /// <summary>
    /// Get feedback statistic detail criteria.
    /// </summary>
    public class GetFeedbackStatisticDetailCriteria : CriteriaBase
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

        /// <summary>
        /// Gets the stored procedure parameters.
        /// </summary>
        /// <returns>The hash of parameters.</returns>
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
                { "fromduration", FromDuration == null ? string.Empty : FromDuration },
                { "toduration", ToDuration == null ? string.Empty : ToDuration }
            };
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && (ToDate - FromDate).Days <= 365;
        }
    }
}
