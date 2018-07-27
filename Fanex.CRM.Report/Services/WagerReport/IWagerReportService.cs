namespace Fanex.CRM.Report.Services.WagerReport
{
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Models.WagerReport;

    public interface IWagerReportService
    {
        WagerReportModel GetWagerReport(GetWagerReportCriteria criteria);

        void ExportWagerReportExcel(GetWagerReportCriteria criteria);
    }
}
