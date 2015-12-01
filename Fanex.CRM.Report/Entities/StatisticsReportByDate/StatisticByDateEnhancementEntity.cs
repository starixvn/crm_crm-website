// -----------------------------------------------------------------------
// <copyright file="StatisticByDateEnhancementEntity.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Entities.StatisticsReportByDate
{
    using System;

    public class StatisticByDateEnhancementEntity : StatisticByDateEntity
    {
        public int CommunicationId { get; set; }

        public string CommunicationName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Total { get; set; }
    }
}
