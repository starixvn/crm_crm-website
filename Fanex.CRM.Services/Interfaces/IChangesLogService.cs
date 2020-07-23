namespace Fanex.CRM.Services.Interfaces
{
    using Fanex.CRM.Criteria.PicAndHeadline;
    using Fanex.CRM.Models;
    using Fanex.CRM.Models.PicAndHeadline;

    public interface IChangesLogService
    {
        BaseReport<ChangesLogInfo> GetHeadlineDAReport(GetHeadlineDALogReportCriteria criteria);

        BaseReport<ChangesLogInfo> GetPicReport(GetPicLogReportCriteria criteria);
    }
}