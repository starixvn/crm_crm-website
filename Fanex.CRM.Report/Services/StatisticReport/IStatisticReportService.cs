namespace Fanex.CRM.Report.Services.StatisticReport
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Models.StatisticReport;
    using System.Collections.Generic;

    public interface IStatisticReportService
    {
        List<Setting> AllPeriodType();

        List<Setting> AllMainCategory();

        StatisticReportInfo StatisticReport(GetStatisticReportCriteria criteria);

        void ExportStatisticReport(GetStatisticReportCriteria criteria);
    }
}
