namespace Fanex.CRM.Feedback.Repositories
{
    using Criteria;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Models;

    public class MultipleLogsRepository : BaseRepository, IMultipleLogsRepository
    {
        public Result InsertMultipleLogs(InsertMultipleLogsCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
