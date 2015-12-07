// -----------------------------------------------------------------------
// <copyright file="ILogStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.LogStatistic
{
    using Fanex.CRM.Report.Criteria.LogStatistic;
    using Fanex.CRM.Report.Models.LogStatistic;

    public interface ILogStatisticService
    {
        LogStatisticViewModel GetReportData(LogStatisticCriteria criteria);
    }
}
