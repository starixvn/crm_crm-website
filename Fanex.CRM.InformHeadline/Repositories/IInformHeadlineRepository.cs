// -----------------------------------------------------------------------
// <copyright file="IInformHeadlineRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Repositories
{
    using System.Collections.Generic;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInformHeadlineRepository
    {
        IEnumerable<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria);

        Result UpdateInformStatus(UpdateInformStatusCriteria criteria);

        IEnumerable<NewInform> GetNewInforms(GetNewInformsCriteria criteria);

        IEnumerable<InformStatus> GetInformStatuses(GetInformStatusesCriteria criteria);

        Result UpdateContentInformHeadline(UpdateContentInformHeadlineCriteria criteria);

        Result InsertInformHeadline(InsertInformHeadlineCriteria criteria);

        Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria);

        IEnumerable<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria);

        TitleDetail GetTileDetail(GetTitleDetailCriteria criteria);

        IEnumerable<UpdateInform> GetContactInforms(GetContactInformsCriteria criteria);
    }
}
