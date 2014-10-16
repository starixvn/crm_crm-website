// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.DailyStatisticsReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Daily Statistics Entity.
    /// </summary>
    [Serializable]
    public class DailyStatisticsEntity
    {
        public int MainCategoryId { get; set; }

        public string MainCategoryName { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public List<int> NumberOfIssues { get; set; }
    }
}
