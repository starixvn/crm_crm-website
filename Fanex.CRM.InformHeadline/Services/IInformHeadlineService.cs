namespace Fanex.CRM.InformHeadline.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Excel;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Setting.Models;

    public interface IInformHeadlineService
    {
        IEnumerable<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria);

        CommandResult UpdateInformStatus(UpdateInformStatusCriteria criteria);

        IEnumerable<NewInform> GetNewInforms(GetNewInformsCriteria criteria);

        dynamic GetNewInformSettings();

        dynamic GetContactInformSettings();

        IEnumerable<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria);

        TitleDetail GetTitleDetail(int titleId);

        CommandResult UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria);

        CommandResult InsertInformHeadline(InsertInformHeadlineCriteria criteria);

        CommandResult UpdateInformHeadline(UpdateInformHeadlineCriteria criteria);

        IEnumerable<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria);

        IEnumerable<InformStatus> GetInformStatuses(bool isIncludeKeptForRefence = false);

        IEnumerable<Status> GetProductStatuses();

        IEnumerable<Station> GetStations();

        Tuple<IEnumerable<RemovedAccountLog>, int> GetRemovedAccountLogs(GetRemovedAccountLogsCriteria criteria);

        Tuple<IEnumerable<RemovedAccountLogDetails>, int> GetRemovedAccountLogDetails(GetRemovedAccountLogDetailsCriteria criteria);

        InformHeadlineExcel GetInformHeadlineExcel(GetInformHeadlinesCriteria criteria, string fileName);

        NewInformHeadlineExcel GetNewInformHeadlineExcel(GetNewInformsCriteria criteria, string fileName);

        UpdateInformHeadlineExcel GetUpdateInformHeadlineExcel(GetUpdateInformsCriteria criteria, string fileName);

        UpdateInformHeadlineExcel GetContactInformHeadlineExcel(GetContactInformsCriteria criteria, string fileName);
    }
}