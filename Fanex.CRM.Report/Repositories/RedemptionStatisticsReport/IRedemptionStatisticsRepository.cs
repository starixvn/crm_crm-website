// --------------------------------------------------------------------------
// <copyright file="IRedemptionStatisticsRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.12.09">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.RedemptionStatisticsReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.RedemptionStatisticsReport;
    using Fanex.CRM.Report.Entities.RedemptionStatistics;

    public interface IRedemptionStatisticsRepository
    {
        IEnumerable<RedemptionStatisticsEntity> GetRedemptionSummary(GetRedemptionStatisticsSummaryCriteria criteria);

        IEnumerable<RedemptionStatisticsDetailEntity> GetRedemptionDetail(GetRedemptionStatisticsDetailCriteria criteria);
    }
}
