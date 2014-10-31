// --------------------------------------------------------------------------
// <copyright file="StatisticsReportByDateModel.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Weekly Statistics Report Information.
    /// </summary>
    [Serializable]
    public class StatisticsReportByDateModel : CategoryIssuesInfo
    {
        public List<BaseModel> CommunicationTools { get; set; }
    }
}
