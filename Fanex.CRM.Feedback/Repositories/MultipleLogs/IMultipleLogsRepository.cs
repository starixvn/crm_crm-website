namespace Fanex.CRM.Feedback.Repositories
{
    using Common.Models;
    using Criteria;

    public interface IMultipleLogsRepository
    {
        BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria);
    }
}