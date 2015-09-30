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

        IEnumerable<SiteInfo> GetAllSites();

        IEnumerable<SettingInfo> GetAllTitles();

        IEnumerable<CountryInfo> GetAllCountries();

        IEnumerable<SettingInfo> GetAllLanguages();

        IEnumerable<SettingInfo> GetAllCustomerTypes();

        IEnumerable<SettingInfo> GetAllVIPTypes();

        void ExportCustomerReport(GetCustomerReportCriteria criteria);
    }
}
