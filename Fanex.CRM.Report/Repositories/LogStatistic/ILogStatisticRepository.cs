// -----------------------------------------------------------------------
// <copyright file="ILogStatisticRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.LogStatistic
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.LogStatistic;
    using Fanex.CRM.Report.Entities.LogStatistic;

    public interface ILogStatisticRepository
    {
        IEnumerable<LogStatisticEntity> GetReportData(LogStatisticCriteria criteria);
    }
}
