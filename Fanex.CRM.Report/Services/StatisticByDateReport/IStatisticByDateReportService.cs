// --------------------------------------------------------------------------
// <copyright file="IStatisticByDateReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.StatisticByDateReport
{
    using Fanex.CRM.Report.Criteria.StatisticByDateReport;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using System.Collections.Generic;

    public interface IStatisticByDateReportService
    {
        StatisticsReportByDateModel GetMainReport(GetMainReportCriteria criteria);

        void ExportMainReport(GetMainReportCriteria criteria);

        StatisticsDetailReportByDateModel DetailReport(GetDetailReportCriteria criteria);

        void ExportDetailReport(
            GetDetailReportCriteria criteria,
            IEnumerable<Common.Models.Setting> mainCategory,
            IEnumerable<Common.Models.Setting> communicationToolList,
            IEnumerable<Common.Models.SettingRelation> extensionList,
            IEnumerable<Common.Models.Product> productList,
            IEnumerable<Common.Models.Product> subProductList);
    }
}
