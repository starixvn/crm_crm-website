// --------------------------------------------------------------------------
// <copyright file="GetFeedbackStatisticDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.15">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.FeedbackStatisticReport
{
    using System;
    using System.Collections;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get feedback statistic detail criteria.
    /// </summary>
    public class GetFeedbackStatisticDetailCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDuration { get; set; }

        public string ToDuration { get; set; }

        public int Type { get; set; }

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
                { "type", Type },
                { "maincategoryid", MainCategoryId },
                { "subcategoryid", SubCategoryId },
                { "valueid", ValueId },
                { "fromdate", FromDate },
                { "todate", ToDate },
                { "fromduration", FromDuration == null ? string.Empty : FromDuration },
                { "toduration", ToDuration == null ? string.Empty : ToDuration }
            };
        }
    }
}
