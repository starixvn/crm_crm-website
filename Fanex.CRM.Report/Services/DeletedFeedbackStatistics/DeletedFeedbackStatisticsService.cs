// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.DeletedFeedbackStatistics
{
    using System.Linq;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DeletedFeedbackStatisticsService : IDeletedFeedbackStatisticsService
    {
        private readonly IDeletedFeedbackStatisticsRepository _deletedFeedbackStatisticsRepository;

        public DeletedFeedbackStatisticsService()
            : this(null)
        {
        }

        public DeletedFeedbackStatisticsService(IDeletedFeedbackStatisticsRepository deletedFeedbackStatisticsRepository)
        {
            _deletedFeedbackStatisticsRepository = deletedFeedbackStatisticsRepository ?? new DeletedFeedbackStatisticsRepository();
        }

        public DeletedFeedbackStatisticsViewModel GetReport(DeletedFeedbackStatisticsCriteria criteria)
        {
            var report = new DeletedFeedbackStatisticsViewModel { Name = Labels.DeletedFeedbackStatistics };

            var deletedFeedbacks = _deletedFeedbackStatisticsRepository.GetDeletedFeedbacks(criteria);

            var deletedFeedbackByHandler = deletedFeedbacks.GroupBy(p => p.Handler);

            report.DeletedFeedbackByHandlers = deletedFeedbackByHandler.Select(p =>
                    new DeletedFeedbackByHandler
                    {
                        Handler = p.Key,
                        DeletedFeedbacks = p,
                        GrandTotal = p.Count()
                    });

            return report;
        }
    }
}
