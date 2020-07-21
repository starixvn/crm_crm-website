namespace Fanex.CRM.Services.Interfaces
{
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;

    public interface IChangeLogService
    {
        BaseReport<ChangeLogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria);
    }
}