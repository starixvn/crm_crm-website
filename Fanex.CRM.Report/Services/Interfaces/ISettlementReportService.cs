// --------------------------------------------------------------------------
// <copyright file="ISettlementReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Interfaces
{
    using Fanex.CRM.Report.Criteria.SettlementReport;
    using Fanex.CRM.Report.Models.SettlementReport;

    public interface ISettlementReportService
    {
        SettlementReportInfo GetSettlementReport(GetSettlementReportCriteria criteria);

        void ExportSettlementReport(GetSettlementReportCriteria criteria);
    }
}
