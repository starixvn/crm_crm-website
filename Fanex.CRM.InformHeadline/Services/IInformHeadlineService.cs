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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInformHeadlineService
    {
        IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria);

        InformHeadlineExcel GetInformHeadlineExcel(GetInformHeadlineCriteria criteria, string fileName);

        Result UpdateInformStatus(UpdateInformHeadlineCriteria criteria);
    }
}
