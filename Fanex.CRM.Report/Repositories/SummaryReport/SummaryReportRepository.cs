namespace Fanex.CRM.Report.Repositories.SummaryReport
{
    using Fanex.CRM.Report.Entities.SummaryReport;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class SummaryReportRepository : ISummaryReportRepository
    {
        public IEnumerable<SummaryRawData> GetSummaryReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<SummaryRawData>(criteria.GetSpParams());
            }
        }
    }
}
