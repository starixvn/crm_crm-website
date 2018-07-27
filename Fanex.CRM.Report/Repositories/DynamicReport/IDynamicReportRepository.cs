namespace Fanex.CRM.Report.Repositories.DynamicReport
{
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using System.Collections.Generic;

    public interface IDynamicReportRepository
    {
        IEnumerable<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria);
    }
}
