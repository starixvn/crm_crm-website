namespace Fanex.CRM.Feedback.Repositories
{
    using Common.Models;
    using Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IMultipleLogsRepository
    {
        Result InsertMultipleLogs(InsertMultipleLogsCriteria criteria);

        Result UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria);

        BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria);

        Result UpdateMultipleLogStatuses(UpdateMultipleLogStatusesCriteria criteria);
    }
}
