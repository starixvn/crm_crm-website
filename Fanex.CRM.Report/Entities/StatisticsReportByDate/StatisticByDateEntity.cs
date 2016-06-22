// --------------------------------------------------------------------------
// <copyright file="StatisticByDateEntity.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.StatisticsReportByDate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Statistics Entity By Date.
    /// </summary>
    [Serializable]
    public class StatisticByDateEntity
    {
        public int MainCategoryId { get; set; }

        public string MainCategoryName { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public List<int> NumberOfIssues { get; set; }
    }
}
