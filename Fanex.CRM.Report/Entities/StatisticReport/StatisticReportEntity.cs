// --------------------------------------------------------------------------
// <copyright file="StatisticReportEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.StatisticReport
{
    using System;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Statistic Report Entity.
    /// </summary>
    [Serializable]
    public class StatisticReportEntity
    {
        public DateTime DateTime { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int NumberOfIssues { get; set; }
    }
}
