// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsDetailReportInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.DailyStatisticsReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Daily Statistics Detail Information.
    /// </summary>
    [Serializable]
    public class DailyStatisticsDetailReportInfo : BaseModel
    {
        public List<DailyStatisticsDetailInfo> ChildInfo { get; set; }

        public int TotalChildInfo { get; set; }

        public int StartChildIndex { get; set; }
    }
}
