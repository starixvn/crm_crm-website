// --------------------------------------------------------------------------
// <copyright file="SettlementReportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.SettlementReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Settlement Report Information.
    /// </summary>
    [Serializable]
    public class SettlementReportInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public int NumberOfIssues { get; set; }

        public List<SettlementReportInfo> ChildReports { get; set; }

        public SettlementReportInfo Total { get; set; }

        public int TotalChildReports { get; set; }

        public SettlementReportInfo ChartInfo { get; set; }
    }
}
