// --------------------------------------------------------------------------
// <copyright file="IStatisticByDateRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.StatisticByDateReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;

    public interface IStatisticByDateRepository
    {
        StatisticsInfoByDateRepositoryResultModel MainReport(GetMainReportCriteria criteria);

        List<StatisticsDetailbyDateModel> DetailReport(GetDetailReportCriteria criteria);
    }
}
