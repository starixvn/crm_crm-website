// --------------------------------------------------------------------------
// <copyright file="ICustomerReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.CRM.Setting.Models;

    public interface ICustomerReportService
    {
        CustomerReportInfo GetCustomerReport(GetCustomerReportCriteria criteria);

        List<SiteInfo> GetAllSite();

        List<SettingInfo> GetAllTitle();

        List<CountryInfo> GetAllCountry();

        List<SettingInfo> GetAllLanguage();

        List<SettingInfo> GetAllCustomerType();

        List<SettingInfo> GetAllVIPType();

        void ExportCustomerReport(GetCustomerReportCriteria criteria);
    }
}
