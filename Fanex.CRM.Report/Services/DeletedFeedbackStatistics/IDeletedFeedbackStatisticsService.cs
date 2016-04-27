// -----------------------------------------------------------------------
// <copyright file="IDeletedFeedbackStatisticsService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.DeletedFeedbackStatistics
{
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;

    public interface IDeletedFeedbackStatisticsService
    {
        DeletedFeedbackStatisticsViewModel GetReport(DeletedFeedbackStatisticsCriteria criteria);
    }
}
