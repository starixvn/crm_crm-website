// --------------------------------------------------------------------------
// <copyright file="ICasinoReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CasinoReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;
    using Fanex.Data.Repository;

    public interface ICasinoReportRepository
    {
        CasinoDailySummary GetCasinoDailySummary(ICriteria criteria);

        CasinoWeeklySummary GetCasinoWeeklySummary(ICriteria criteria);

        IList<CasinoDetailEntity> GetCasinoDetails(ICriteria criteria);
    }
}
