// --------------------------------------------------------------------------
// <copyright file="StatisticsDetailReportByDateModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.StatisticsReportByDate
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    /// <summary>
    /// Weekly Statistics Detail Information.
    /// </summary>
    [Serializable]
    public class StatisticsDetailReportByDateModel : BaseModel
    {
        public List<StatisticsDetailbyDateModel> ChildReports { get; set; }

        public int TotalChildReports { get; set; }

        public int StartChildIndex { get; set; }
    }
}
