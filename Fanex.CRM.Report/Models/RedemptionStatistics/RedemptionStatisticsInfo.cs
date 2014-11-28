// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticsInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.26">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.RedemptionStatistics
{
    using System;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using System.Collections.Generic;

    /// <summary>
    /// Redemption Statistic Report Information.
    /// </summary>
    [Serializable]
    public class RedemptionStatisticsInfo
    {
        public string Title { get; set; }

        public List<RedemptionStatisticsEntity> ReportData { get; set; }

        public List<RedemptionStatisticsEntity> TotalData { get; set; }

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
