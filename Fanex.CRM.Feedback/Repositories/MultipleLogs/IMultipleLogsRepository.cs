namespace Fanex.CRM.Feedback.Repositories
{
    using Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IMultipleLogsRepository
    {
        Result InsertMultipleLogs(InsertMultipleLogsCriteria criteria);

        Result UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria);
    }
}
