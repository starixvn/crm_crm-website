// --------------------------------------------------------------------------
// <copyright file="IStatisticReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Models.StatisticReport;

    public interface IStatisticReportService
    {
        List<SettingInfo> GetAllPeriodType();

        List<SettingInfo> GetAllMainCategory();

        StatisticReportInfo GetStatisticReport(GetStatisticReportCriteria criteria);

        void ExportStatisticReport(GetStatisticReportCriteria criteria);
    }
}
