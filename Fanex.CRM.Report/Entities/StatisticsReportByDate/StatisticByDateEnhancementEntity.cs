// -----------------------------------------------------------------------
// <copyright file="StatisticByDateEnhancementEntity.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Entities.StatisticsReportByDate
{
    using System;

    public class StatisticByDateEnhancementEntity
    {
        public int MainCategoryId { get; set; }

        public string MainCategory { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategory { get; set; }

        public int CommunicationId { get; set; }

        public string Communication { get; set; }

        public int ProductId { get; set; }

        public string Product { get; set; }

        public int Total { get; set; }
    }
}
