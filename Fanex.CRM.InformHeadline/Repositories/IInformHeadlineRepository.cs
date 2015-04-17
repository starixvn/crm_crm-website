// -----------------------------------------------------------------------
// <copyright file="IInformHeadlineRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInformHeadlineRepository
    {
        IList<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria);

        Result UpdateInformStatus(UpdateInformStatusCriteria criteria);

        IList<NewInform> GetNewInforms(GetNewInformsCriteria criteria);

        IList<InformStatus> GetInformStatuses(GetInformStatusesCriteria criteria);

        Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria);

        Result InsertInformHeadline(InsertInformHeadlineCriteria criteria);

        IList<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria);

        TitleDetail GetTileDetail(GetTitleDetailCriteria criteria);
    }
}
