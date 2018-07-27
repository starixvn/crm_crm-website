namespace Fanex.CRM.Report.Services.DeletedFeedbackStatistics
{
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Report.Criteria.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Models.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Repositories.DeletedFeedbackStatistics;
    using Fanex.CRM.Report.Resources;
    using System.Linq;

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
            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

            var deletedFeedbacks = _deletedFeedbackStatisticsRepository.GetDeletedFeedbacks(criteria);

            var deletedFeedbackByHandler = deletedFeedbacks.GroupBy(p => p.Handler);

            return new DeletedFeedbackStatisticsViewModel
            {
                Name = Labels.DeletedFeedbackStatistics,
                GrandTotal = deletedFeedbacks.Count(),
                DeletedFeedbackByHandlers = deletedFeedbackByHandler.Select(p =>
                    new DeletedFeedbackByHandler
                    {
                        Handler = p.Key,
                        DeletedFeedbacks = p
                    })
            };
        }
    }
}