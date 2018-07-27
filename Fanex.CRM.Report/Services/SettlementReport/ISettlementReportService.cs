namespace Fanex.CRM.Report.Services.SettlementReport
{
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Models.SettlementReport;

    public interface ISettlementReportService
    {
        SettlementReportInfo GetSettlementReport(GetSettlementReportCriteria criteria);

        void ExportSettlementReport(GetSettlementReportCriteria criteria);
    }
}
