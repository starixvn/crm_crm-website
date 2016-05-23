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
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Entities.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;

    public interface ICasinoReportRepository
    {
        CasinoDailySummary GetCasinoDailySummary(GetCasinoSummaryCriteria criteria);

        CasinoWeeklySummary GetCasinoWeeklySummary(GetCasinoSummaryCriteria criteria);

        IEnumerable<CasinoDetailEntity> GetCasinoDetails(GetCasinoDetailCriteria criteria);
    }
}
