// --------------------------------------------------------------------------
// <copyright file="IWeeklyStatisticsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.WeeklyStatisticsReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.WeeklyStatisticsReport;
    using Fanex.CRM.Report.Models.WeeklyStatisticsReport;

    public interface IWeeklyStatisticsRepository
    {
        WeeklyStatisticsInfoRepositoryResult MainReport(GetMainReportCriteria criteria);

        List<WeeklyStatisticsDetailInfo> DetailReport(GetDetailReportCriteria criteria);
    }
}
