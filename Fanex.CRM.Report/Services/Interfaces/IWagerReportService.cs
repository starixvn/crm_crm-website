// --------------------------------------------------------------------------
// <copyright file="IWagerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Interfaces
{
    using Fanex.CRM.Report.Models.WagerReport;
    using Fanex.CRM.Report.Criteria.WagerReport;

    /// <summary>
    /// IWagerReportService interface.
    /// </summary>
    public interface IWagerReportService
    {
        WagerReportModel GetWagerReport(GetWagerReportCriteria criteria);

        void ExportWagerReport(GetWagerReportCriteria criteria);
    }
}
