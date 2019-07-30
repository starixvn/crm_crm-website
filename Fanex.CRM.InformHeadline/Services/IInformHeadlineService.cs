namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Excel;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Setting.Models;

    public interface IInformHeadlineService
    {
        IEnumerable<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria);

        NewInformHeadlineExcel GetNewInformHeadlineExcel(GetNewInformsCriteria criteria, string fileName);

        UpdateInformHeadlineExcel GetUpdateInformHeadlineExcel(GetUpdateInformsCriteria criteria, string fileName);

        UpdateInformHeadlineExcel GetContactInformHeadlineExcel(GetContactInformsCriteria criteria, string fileName);

        CommandResult UpdateInformStatus(UpdateInformStatusCriteria criteria);

        IEnumerable<NewInform> GetNewInforms(GetNewInformsCriteria criteria);

        dynamic GetNewInformSettings();

        dynamic GetContactInformSettings();

        IEnumerable<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria);

        TitleDetail GetTitleDetail(GetTitleDetailCriteria criteria);

        CommandResult UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria);

        CommandResult InsertInformHeadline(InsertInformHeadlineCriteria criteria);

        CommandResult UpdateInformHeadline(UpdateInformHeadlineCriteria criteria);

        IEnumerable<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria);

        IEnumerable<InformStatus> GetInformStatuses(bool isIncludeKeptForRefence = false);

        IEnumerable<Status> GetProductStatuses();

        IEnumerable<Station> GetStations();

        IEnumerable<RemovedAccountLog> GetRemovedAccountLogs(int titleId);
    }
}