namespace Fanex.CRM.Communication.Services
{
    using System.Collections.Generic;
    using Criteria;
    using Infrastructure.Models;

    public interface IMultipleLogsService
    {
        IList<BaseModel> AllStatusForInsert { get; }

        IList<BaseModel> AllStatusForEdit { get; }

        Result InsertLog(InsertMultipleLogsCriteria criteria);
    }
}
