// -----------------------------------------------------------------------
// <copyright file="LogStatistic.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Entities.LogStatistics
{
    using System;

    public class LogStatisticsEntity
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
