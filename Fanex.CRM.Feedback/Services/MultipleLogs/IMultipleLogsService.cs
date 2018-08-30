namespace Fanex.CRM.Feedback.Services
{
    using Common.Models;
    using Criteria;
    using Infrastructure.Models;
    using System.Collections.Generic;

    public interface IMultipleLogsService
    {
        IList<BaseModel> AllStatusForInsert { get; }

        IList<BaseModel> AllStatusForEdit { get; }

        Result InsertMultpleLogs(InsertMultipleLogsCriteria criteria);

        Result UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria);

        BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria);

        Result UpdateMultipleLogStatuses(UpdateMultipleLogStatusesCriteria criteria);
    }
}
