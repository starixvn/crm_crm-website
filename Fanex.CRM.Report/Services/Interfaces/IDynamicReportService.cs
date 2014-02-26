// --------------------------------------------------------------------------
// <copyright file="IDynamicReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Setting.Models;

    public interface IDynamicReportService
    {
        List<SettingInfo> GetAllPeriodType();

        List<SettingInfo> GetAllProduct();

        List<SiteInfo> GetAllSite();

        List<SettingInfo> GetAllStatus();

        List<SettingInfo> GetAllResponse();

        List<SettingInfo> GetAllRole();

        List<SettingInfo> GetAllCommunicationTool();

        List<SettingInfo> GetAllMainCategory();

        List<SettingInfo> GetAllExtension();

        DynamicReport GetDynamicReport(GetDynamicReportCriteria criteria);
    }
}
