// --------------------------------------------------------------------------
// <copyright file="GetFeedbackStatisticCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Criteria.FeedbackStatisticReport
{
    using System;
    using System.Collections;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get feedback statistic criteria.
    /// </summary>
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
