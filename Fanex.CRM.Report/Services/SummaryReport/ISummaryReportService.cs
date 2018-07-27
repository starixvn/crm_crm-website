namespace Fanex.CRM.Report.Services.SummaryReport
{
    using Fanex.CRM.Report.Criteria.SummaryReport;
    using Fanex.CRM.Report.Models.SummaryReport;

    public interface ISummaryReportService
    {
        dynamic AllSettings();

        SummaryReportModel GetSummaryReport(GetSummaryReportCriteria criteria);

        void ExportSummaryReportExcel(GetSummaryReportCriteria criteria);
    }
}