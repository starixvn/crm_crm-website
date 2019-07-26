namespace Fanex.CRM.Communication.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Communication.Criteria;

    public interface ICommunicationLogRepository
    {
        IEnumerable<BaseCommunicationLog> GetLogsByCustomerId(GetLogsByCustomerIdCriteria criteria);

        BaseCommunicationLog GetLogsByRunningNumber(GetLogByRunningNumberCriteria criteria);
    }
}