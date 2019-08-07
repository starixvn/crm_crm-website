namespace Fanex.CRM.InformHeadline.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;

    public interface IInformHeadlineRepository
    {
        CommandResult InsertInformHeadline(InsertInformHeadlineCriteria criteria);

        IEnumerable<RemovedAccountLog> GetRemovedAccountLogs(GetRemovedAccountLogsCriteria criteria);

        IEnumerable<RemovedAccountLogDetails> GetRemovedAccountLogDetails(GetRemovedAccountLogDetailsCriteria criteria);
    }
}