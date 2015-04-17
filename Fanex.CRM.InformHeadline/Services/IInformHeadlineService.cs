// -----------------------------------------------------------------------
// <copyright file="IInformHeadlineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.InformHeadline.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CRM.InformHeadline.Criteria;
    using Fanex.CRM.InformHeadline.Excel;
    using Fanex.CRM.InformHeadline.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInformHeadlineService
    {
        IList<InformHeadline> GetInforms(GetInformHeadlinesCriteria criteria);

        InformHeadlineExcel GetInformHeadlineExcel(GetInformHeadlinesCriteria criteria, string fileName);

        Result UpdateInformStatus(UpdateInformStatusCriteria criteria);

        IList<NewInform> GetNewInforms(GetNewInformsCriteria criteria);

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        dynamic GetNewInformSettings();

        IList<UpdateInform> GetUpdateInforms(GetUpdateInformsCriteria criteria);

        TitleDetail GetTitleDetail(GetTitleDetailCriteria criteria);

        Result UpdateInformHeadline(UpdateInformHeadlineCriteria criteria);

        Result InsertInformHeadline(InsertInformHeadlineCriteria criteria);
    }
}
