// --------------------------------------------------------------------------
// <copyright file="StatisticReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2014.12.18">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.StatisticReport
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Statistic report repository.
    /// </summary>
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
