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
    using System.Data;
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
        /// <summary>
        /// Gets the statistic report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The statistic report list.</returns>
        public List<StatisticReportEntity> GetStatisticReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    List<StatisticReportEntity> statistics = new List<StatisticReportEntity>(256);
                    while (reader.Read())
                    {
                        statistics.Add(ParseStatisticReportEntity(reader));    
                    }

                    return statistics;
                }
            }
        }

        private StatisticReportEntity ParseStatisticReportEntity(IDataReader reader)
        {
            return new StatisticReportEntity 
            {
                DateTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["FeedbackDate"])),
                CountryId = Converter.ToInt(reader["CountryId"]),
                CountryName = Converter.ToString2(reader["CountryName"]),
                NumberOfIssues = Converter.ToInt(reader["NoOfIssues"])
            };
        }
    }
}
