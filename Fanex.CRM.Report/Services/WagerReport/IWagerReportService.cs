// --------------------------------------------------------------------------
// <copyright file="IWagerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.WagerReport
{
    using Fanex.CRM.Report.Criteria.WagerReport;
    using Fanex.CRM.Report.Models.WagerReport;

    /// <summary>
    /// IWagerReportService interface.
    /// </summary>
    public interface IWagerReportService
    {
        WagerReportModel GetWagerReport(GetWagerReportCriteria criteria);

        void ExportWagerReportExcel(GetWagerReportCriteria criteria);
    }
}
