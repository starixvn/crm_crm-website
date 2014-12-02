// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticsEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.26">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.RedemptionStatistics
{
    using System;

    /// <summary>
    /// Redemption Statistic Report Entity.
    /// </summary>
    [Serializable]
    public class RedemptionStatisticsEntity
    {
        public string Subject { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }
    }
}
