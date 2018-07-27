namespace Fanex.CRM.Report.Repositories.WagerReport
{
    using Fanex.CRM.Report.Entities.WagerReport;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class WagerReportRepository : IWagerReportRepository
    {
        public IEnumerable<WagerRawData> GetWagerReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<WagerRawData>(criteria.GetSpParams());
            }
        }
    }
}
