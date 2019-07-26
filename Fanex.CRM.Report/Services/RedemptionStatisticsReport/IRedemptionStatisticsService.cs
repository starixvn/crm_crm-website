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