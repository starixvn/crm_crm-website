// --------------------------------------------------------------------------
// <copyright file="StatisticReportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.StatisticReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Statistic Report Information.
    /// </summary>
    [Serializable]
    public class StatisticReportInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int ReportTypeId { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public bool HasData { get; set; }

        public CountryInfo CountryInfo { get; set; }

        public int NumberOfIssues { get; set; }

        public List<StatisticReportInfo> ChildReports { get; set; }

        public StatisticReportInfo Total { get; set; }

        public int TotalChildReports { get; set; }
        
        public StatisticReportInfo ChartInfo { get; set; }
    }
}
