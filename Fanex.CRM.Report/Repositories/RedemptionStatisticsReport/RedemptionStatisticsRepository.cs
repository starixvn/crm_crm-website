// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticsRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.12.09">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.RedemptionStatisticsReport
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;
    using Fanex.Data;

    /// <summary>
    /// Redemption statistics repository.
    /// </summary>
    public class RedemptionStatisticsRepository : IRedemptionStatisticsRepository
    {
        private const int SummaryByYear = 2;

        public IEnumerable<RedemptionStatisticsEntity> GetRedemptionSummary(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var redemptions = new List<RedemptionStatisticsEntity>();
                    while (reader.Read())
                    {
                        var entity = new RedemptionStatisticsEntity
                        {
                            ClaimTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])),
                            Subject = Converter.ToString2(reader["Subject"]),
                            CurrencyId = Converter.ToInt(reader["CurrencyId"]),
                            Currency = Converter.ToString2(reader["Currency"]),
                            Amount = Converter.ToDecimal(reader["Amount"])
                        };

                        entity.Year = entity.ClaimTime.Year;
                        if (criteria.ReportBy != SummaryByYear)
                        {
                            entity.Month = entity.ClaimTime.Month;
                        }

                        redemptions.Add(entity);
                    }

                    return redemptions;
                }
            }
        }

        public IEnumerable<RedemptionStatisticsDetailEntity> GetRedemptionDetail(GetRedemptionStatisticsDetailCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var data = db.Query<RedemptionStatisticsDetailEntity>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return data;
            }
        }
    }
}
