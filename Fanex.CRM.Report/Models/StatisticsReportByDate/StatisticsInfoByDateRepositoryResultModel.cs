// --------------------------------------------------------------------------
// <copyright file="StatisticsInfoByDateRepositoryResultModel.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;

    /// <summary>
    /// Weekly Statistics Information Repository Result.
    /// </summary>
    [Serializable]
    public class StatisticsInfoByDateRepositoryResultModel
    {
        public IList<StatisticByDateEntity> RawData { get; set; }

        public IList<BaseModel> CommunicationTools { get; set; }
    }
}
