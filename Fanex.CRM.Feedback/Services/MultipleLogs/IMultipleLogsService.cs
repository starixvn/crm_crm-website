namespace Fanex.CRM.Feedback.Services
{
    using System.Collections.Generic;
    using Common.Models;
    using Criteria;

    public interface IMultipleLogsService
    {
        IList<BaseModel> AllStatusForInsert { get; }

        IList<BaseModel> AllStatusForEdit { get; }

        IEnumerable<BaseModel> CheckNotExistCommunicationToolSetting(CheckNotExistCommunicationToolSettingCriteria criteria);

        CommandResult InsertMultpleLogs(InsertMultipleLogsCriteria criteria);

        CommandResult UpdateMultipleLogs(UpdateMultipleLogsCriteria criteria);

        BaseCommunicationLog GetLogByFeedbackId(GetLogByFeedbackIdCriteria criteria);

        CommandResult UpdateMultipleLogStatuses(UpdateMultipleLogStatusesCriteria criteria);
    }
}