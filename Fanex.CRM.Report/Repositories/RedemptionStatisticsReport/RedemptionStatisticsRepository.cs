// --------------------------------------------------------------------------
// <copyright file="RedemptionStatisticsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.12.09">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.RedemptionStatisticsReport
{
    using System.Collections.Generic;
    using System.Linq;
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
        /// <summary>
        /// Gets the redemption detail.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of Redemption statistics.</returns>
        public IList<RedemptionStatisticsEntity> GetRedemptionSummary(GetRedemptionStatisticsSummaryCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    RedemptionStatisticsEntity entity;
                    IList<RedemptionStatisticsEntity> redemptions = new List<RedemptionStatisticsEntity>();
                    while (reader.Read())
                    {
                        entity = new RedemptionStatisticsEntity
                        {
                            ClaimTime = DateTimeUtilize.ChangeTimeZoneToClientDateTime(Converter.ToDateTime(reader["ClaimTime"])),
                            Subject = Converter.ToString2(reader["Subject"]),
                            CurrencyId = Converter.ToInt(reader["CurrencyId"]),
                            Currency = Converter.ToString2(reader["Currency"]),
                            Amount = Converter.ToDecimal(reader["Amount"])
                        };

                        entity.Year = entity.ClaimTime.Year;
                        entity.Month = entity.ClaimTime.Month;
                        redemptions.Add(entity);
                    }

                    return redemptions;
                }
            }
        }

        /// <summary>
        /// Gets the redemption detail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of redemption detail.</returns>
        public IList<RedemptionStatisticsDetailEntity> GetRedemptionDetail(GetRedemptionStatisticsDetailCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var rawdata = db.Query<RedemptionStatisticsDetailEntity>(criteria.GetSpParams()).ToList();
                var outputParams = db.GetOuputParams();
                if (outputParams != null && outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return rawdata;
            }
        }
    }
}
