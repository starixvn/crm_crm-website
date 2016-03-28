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
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.Excel.Models;
    using NPOI.SS.UserModel;

    public interface IRedemptionStatisticsService
    {
        List<BaseModel> AllStatus();

        List<BaseModel> AllCurrencyRedemption();

        List<BaseModel> AllReportBy();

        GridOptions GetRedemptionSummaryReport(GetRedemptionStatisticsSummaryCriteria criteria);

        GridOptions GetRedemptionDetailReport(GetRedemptionStatisticsDetailCriteria criteria);

        IWorkbook GenerateSummaryExcelWorkbook(GetRedemptionStatisticsSummaryCriteria criteria);

        IWorkbook GenerateDetailExcelWorkbook(GetRedemptionStatisticsDetailCriteria criteria);
    }
}
