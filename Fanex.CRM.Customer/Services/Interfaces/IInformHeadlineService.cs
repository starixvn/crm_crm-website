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
    using Fanex.CRM.Customer.Criteria.InformHeadlines;
    using Fanex.CRM.Customer.Models.InformHeadlines;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// IInformHeadlineService interface.
    /// </summary>
    public interface IInformHeadlineService
    {
        InformHeadlineSetting GetAllInformHeadlineSettings();

        InformHeadlineStatistic GetInformStatistic(GetInformHeadlineStatisticCriteria criteria);

        void ExportInformStatistic(GetInformHeadlineStatisticCriteria criteria);

        Result CreateTitle(ICriteria criteria);

        IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria);

        void ExportInforms(GetInformHeadlineCriteria criteria);

        Result UpdateInforms(UpdateInformHeadlinesCriteria criteria);
    }
}
