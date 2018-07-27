namespace Fanex.CRM.Report.Repositories.StatisticReport
{
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class StatisticReportRepository : IStatisticReportRepository
    {
        public IList<StatisticReportEntity> GetStatisticReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var statistics = new List<StatisticReportEntity>(256);
                    while (reader.Read())
                    {
                        var entity = new StatisticReportEntity
                        {
                            DateTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])),
                            CountryId = Converter.ToInt(reader["CountryId"]),
                            CountryName = Converter.ToString2(reader["CountryName"]),
                            NumberOfIssues = Converter.ToInt(reader["NoOfIssues"])
                        };
                        statistics.Add(entity);
                    }

                    return statistics;
                }
            }
        }
    }
}
