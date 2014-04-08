// --------------------------------------------------------------------------
// <copyright file="ISummaryReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Interfaces
{
    using Fanex.CRM.Report.Models.SummaryReport;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// ISummaryReportRepository interface.
    /// </summary>
    public interface ISummaryReportRepository
    {
        SummaryReportModel GetSummaryReport(ICriteria criteria); 
    }
}
