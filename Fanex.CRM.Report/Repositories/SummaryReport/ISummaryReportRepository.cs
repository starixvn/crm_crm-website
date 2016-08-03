// --------------------------------------------------------------------------
// <copyright file="ISummaryReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.SummaryReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.SummaryReport;
    using Fanex.Data.Repository;

    /// <summary>
    /// ISummaryReportRepository interface.
    /// </summary>
    public interface ISummaryReportRepository
    {
        IEnumerable<SummaryRawData> GetSummaryReport(ICriteria criteria); 
    }
}
