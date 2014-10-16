// --------------------------------------------------------------------------
// <copyright file="WeeklyStatisticsDetailReportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.WeeklyStatisticsReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Weekly Statistics Detail Information.
    /// </summary>
    [Serializable]
    public class WeeklyStatisticsDetailReportInfo : BaseModel
    {
        public List<WeeklyStatisticsDetailInfo> ChildInfo { get; set; }

        public int TotalChildInfo { get; set; }

        public int StartChildIndex { get; set; }
    }
}
