// --------------------------------------------------------------------------
// <copyright file="DynamicReportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Dynamic Report Information.
    /// </summary>
    [Serializable]
    public class DynamicReportInfo : BaseCommunicationLog
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public int ReportTypeId { get; set; }

        public PeriodRange PeriodRange { get; set; }

        public string Username { get; set; }

        public List<DynamicReportInfo> ChildReports { get; set; }

        public int TotalChildReports { get; set; }

        public int StartChildIndex { get; set; }

        public DynamicReportInfo ChartInfo { get; set; }
    }
}
