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

        Result UpdateInformStatus(UpdateInformHeadlineCriteria criteria);

        IList<DirectAccount> GetDirectAccounts(GetDirectAccountsCriteria criteria);
    }
}
