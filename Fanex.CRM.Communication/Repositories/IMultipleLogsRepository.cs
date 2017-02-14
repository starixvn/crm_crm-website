namespace Fanex.CRM.Communication.Repositories
{
    using Fanex.CRM.Communication.Criteria;
    using Fanex.CRM.Infrastructure.Models;

    public interface IMultipleLogsRepository
    {
        Result InsertLog(InsertMultipleLogsCriteria criteria);
    }
}
