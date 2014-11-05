// --------------------------------------------------------------------------
// <copyright file="ICustomerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Setting.Models;

    public interface ICustomerReportService
    {
        CustomerReportInfo CustomerReport(GetCustomerReportCriteria criteria);

        List<SiteInfo> AllSite();

        List<SettingInfo> AllTitle();

        List<CountryInfo> AllCountry();

        List<SettingInfo> AllLanguage();

        List<SettingInfo> AllCustomerType();

        List<SettingInfo> AllVIPType();

        void ExportCustomerReport(GetCustomerReportCriteria criteria);
    }
}
