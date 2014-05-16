// --------------------------------------------------------------------------
// <copyright file="IInformHeadlineService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// IInformHeadlineService interface.
    /// </summary>
    public interface IInformHeadlineService
    {
        InformHeadlineStatistic GetInformStatistic(GetInformHeadlineStatisticCriteria criteria);

        void ExportInformStatistic(GetInformHeadlineStatisticCriteria criteria);

        Result CreateTitle(ICriteria criteria);

        IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria);

        Result UpdateInforms(UpdateInformHeadlinesCriteria criteria);
    }
}
