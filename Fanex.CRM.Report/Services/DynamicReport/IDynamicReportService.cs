// --------------------------------------------------------------------------
// <copyright file="IDynamicReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DynamicReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Setting.Models;

    public interface IDynamicReportService
    {
        IEnumerable<Setting> GetAllPeriodTypes();

        IEnumerable<Setting> GetAllParentProducts();

        IEnumerable<Setting> GetAllProducts();

        IEnumerable<SiteInfo> GetAllSites();

        IEnumerable<Setting> GetAllStatuses();

        IEnumerable<Setting> GetAllResponses();

        IEnumerable<Setting> GetAllRoles();

        IEnumerable<Setting> GetAllCommunicationTools();

        IEnumerable<Setting> GetAllMainCategories();

        IEnumerable<Setting> GetAllExtensions();

        DynamicReportInfo GetDynamicReport(GetDynamicReportCriteria criteria);

        void ExportDynamicReport(GetDynamicReportCriteria criteria);
    }
}
