// -----------------------------------------------------------------------
// <copyright file="LogStatisticsReportDetailCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Criteria.LogStatistics
{
    using System;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data.Repository;

    public class LogStatisticsReportDetailCriteria : PagingCriteria
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SiteType { get; set; }

        public string ProductId { get; set; }

        public string MainCategoryId { get; set; }

        public string CommunicationId { get; set; }

        [SpParam(Ignored = true)]
        public bool IsGetFullReport { get; set; }

        public override string GetSettingKey()
        {
            return "Report_LogStatisticDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
