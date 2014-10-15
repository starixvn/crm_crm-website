// --------------------------------------------------------------------------
// <copyright file="DailyStatisticsInfoRepositoryResult.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Report.Entities.DailyStatisticsReport;

    /// <summary>
    /// Daily Statistics Information Repository Result.
    /// </summary>
    [Serializable]
    public class DailyStatisticsInfoRepositoryResult
    {
        public List<DailyStatisticsEntity> RawData { get; set; }

        public List<BaseModel> CommunicationTools { get; set; }
    }
}
