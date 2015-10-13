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
        IEnumerable<SettingInfo> GetAllPeriodTypes();

        IEnumerable<SettingInfo> GetAllProducts();

        IEnumerable<SiteInfo> GetAllSites();

        IEnumerable<SettingInfo> GetAllStatuses();

        IEnumerable<SettingInfo> GetAllResponses();

        IEnumerable<SettingInfo> GetAllRoles();

        IEnumerable<SettingInfo> GetAllCommunicationTools();

        IEnumerable<SettingInfo> GetAllMainCategories();

        IEnumerable<SettingInfo> GetAllExtensions();

        DynamicReportInfo GetDynamicReport(GetDynamicReportCriteria criteria);

        void ExportDynamicReport(GetDynamicReportCriteria criteria);
    }
}
