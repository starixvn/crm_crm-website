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
        List<SettingInfo> AllPeriodType();

        List<SettingInfo> AllProduct();

        List<SiteInfo> AllSite();

        List<SettingInfo> AllStatus();

        List<SettingInfo> AllResponse();

        List<SettingInfo> AllRole();

        List<SettingInfo> AllCommunicationTool();

        List<SettingInfo> AllMainCategory();

        List<SettingInfo> AllExtension();

        DynamicReportInfo GetDynamicReport(GetDynamicReportCriteria criteria);

        void ExportDynamicReport(GetDynamicReportCriteria criteria);
    }
}
