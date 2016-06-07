// -----------------------------------------------------------------------
// <copyright file="IInformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Excel;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInformHeadlineService
    {
        IEnumerable<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria);

        InformHeadlineExcel GetInformHeadlineExcel(GetInformHeadlinesCriteria criteria, string fileName);

        NewInformHeadlineExcel GetNewInformHeadlineExcel(GetNewInformsCriteria criteria, string fileName);

        UpdateInformHeadlineExcel GetUpdateInformHeadlineExcel(GetUpdateInformsCriteria criteria, string fileName);

        UpdateInformHeadlineExcel GetContactInformHeadlineExcel(GetContactInformsCriteria criteria, string fileName);

        Result UpdateInformStatus(UpdateInformStatusCriteria criteria);

        IEnumerable<NewInform> GetNewInforms(GetNewInformsCriteria criteria);

        dynamic GetNewInformSettings();

        dynamic GetContactInformSettings();

        IEnumerable<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria);

        TitleDetail GetTitleDetail(GetTitleDetailCriteria criteria);

        Result UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria);

        Result InsertInformHeadline(InsertInformHeadlineCriteria criteria);

        Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria);

        IEnumerable<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria);

        IEnumerable<InformStatus> GetInformStatuses(bool isIncludeKeptForRefence = false);

        IEnumerable<Status> GetProductStatuses();

        IEnumerable<Station> GetStations();
    }
}
