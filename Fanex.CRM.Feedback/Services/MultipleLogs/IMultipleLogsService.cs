namespace Fanex.CRM.Feedback.Services
{
    using System.Collections.Generic;
    using Criteria;
    using Infrastructure.Models;

    public interface IMultipleLogsService
    {
        IList<BaseModel> AllStatusForInsert { get; }

        IList<BaseModel> AllStatusForEdit { get; }

        Result InsertMultpleLogs(InsertMultipleLogsCriteria criteria);

        Result UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria);
    }
}
