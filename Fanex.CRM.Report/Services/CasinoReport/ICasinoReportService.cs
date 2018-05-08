// --------------------------------------------------------------------------
// <copyright file="ICasinoReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.CasinoReport
{
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;
    using System.Collections.Generic;

    public interface ICasinoReportService
    {
        // Casino Daily Statistic.
        CasinoDailySummary GetCasinoDailySummary(GetCasinoSummaryCriteria criteria);

        void ExportCasinoDailySummary(GetCasinoSummaryCriteria criteria);

        CasinoDetail GetCasinoDailyDetail(GetCasinoDetailCriteria criteria);

        void ExportCasinoDailyDetail(
            GetCasinoDetailCriteria criteria,
            IEnumerable<Common.Models.Setting> mainCategory,
            IEnumerable<Common.Models.Setting> communicationToolList,
            IEnumerable<Common.Models.SettingRelation> extensionList,
            IEnumerable<Common.Models.Product> productList,
            IEnumerable<Common.Models.Product> subProductList);

        // Casino Weekly Statistic.
        CasinoWeeklySummary GetCasinoWeeklySummary(GetCasinoSummaryCriteria criteria);

        void ExportCasinoWeeklySummary(GetCasinoSummaryCriteria criteria);

        CasinoDetail GetCasinoWeeklyDetail(GetCasinoDetailCriteria criteria);
    }
}
