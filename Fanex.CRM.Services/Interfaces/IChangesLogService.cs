namespace Fanex.CRM.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;

    public interface IChangesLogService
    {
        BaseReport<ChangeHeadlineDALogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria);

        BaseReport<ChangePicLogInfo> GetPicReport(GetPicLogReportCriteria criteria);

        IEnumerable<BaseModel> GetActionType(GetActionTypeCriteria criteria);
    }
}