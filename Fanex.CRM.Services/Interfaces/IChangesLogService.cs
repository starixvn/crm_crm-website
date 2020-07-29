namespace Fanex.CRM.Services.Interfaces
{
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;
    using Fanex.CRM.Common.Models;
    using System.Collections.Generic;

    public interface IChangesLogService
    {
        BaseReport<ChangeHeadlineDALogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria);

        BaseReport<ChangePicLogInfo> GetPicReport(GetPicLogReportCriteria criteria);

        IEnumerable<BaseModel> GetActionType(GetActionTypeCriteria criteria);
    }
}