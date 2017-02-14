namespace Fanex.CRM.Communication.Repositories
{
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public class MultipleLogsRepository : BaseRepository, IMultipleLogsRepository
    {
        public Result InsertLog(InsertMultipleLogsCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
