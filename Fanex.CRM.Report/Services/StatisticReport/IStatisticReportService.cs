// --------------------------------------------------------------------------
// <copyright file="IStatisticReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.StatisticReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.StatisticReport;
    using Fanex.CRM.Report.Models.StatisticReport;

    public interface IStatisticReportService
    {
        List<Setting> AllPeriodType();

        List<Setting> AllMainCategory();

        StatisticReportInfo StatisticReport(GetStatisticReportCriteria criteria);

        void ExportStatisticReport(GetStatisticReportCriteria criteria);
    }
}
