// --------------------------------------------------------------------------
// <copyright file="WeeklyStatisticsEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.WeeklyStatisticsReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Weekly Statistics Entity.
    /// </summary>
    [Serializable]
    public class WeeklyStatisticsEntity
    {
        public int MainCategoryId { get; set; }

        public string MainCategoryName { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public List<int> NumberOfIssues { get; set; }
    }
}
