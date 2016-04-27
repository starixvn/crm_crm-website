// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackStatisticsViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.DeletedFeedbackStatistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DeletedFeedbackStatisticsViewModel
    {
        public string Name { get; set; }

        public IEnumerable<DeletedFeedbackByHandler> DeletedFeedbackByHandlers { get; set; }

        public int GrandTotal { get; set; }
    }
}