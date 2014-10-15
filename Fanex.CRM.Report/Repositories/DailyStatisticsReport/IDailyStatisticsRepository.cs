// --------------------------------------------------------------------------
// <copyright file="IDailyStatisticsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.DailyStatisticsReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.DailyStatisticsReport;
    using Fanex.CRM.Report.Models.DailyStatisticsReport;

    public interface IDailyStatisticsRepository
    {
        DailyStatisticsInfoRepositoryResult MainReport(GetMainReportCriteria criteria);

        List<DailyStatisticsDetailInfo> DetailReport(GetDetailReportCriteria criteria);
    }
}
