// --------------------------------------------------------------------------
// <copyright file="IRedemptionStatisticsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.11.24">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DynamicReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using Fanex.CRM.Report.Models.RedemptionStatistics;
    using Fanex.Excel.Models;

    public interface IRedemptionStatisticsService
    {
        List<SettingInfo> AllStatus();

        List<SettingInfo> AllCurrencyRedemption();

        List<BaseModel> AllReportBy();

        GridOptions GetRedemptionSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria);

        GridOptions GetRedemptionDetailReport(GetRedemptionStatisticsDetailCriteria criteria);
    }
}
