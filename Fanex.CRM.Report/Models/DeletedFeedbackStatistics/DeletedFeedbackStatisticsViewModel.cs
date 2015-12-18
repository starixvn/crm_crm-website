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
    public class DeletedFeedbackStatisticsViewModel : BaseModel
    {
        public DeletedFeedbackStatisticsViewModel()
        {
            DeletedFeedbackByHandlers = new List<DeletedFeedbackByHandler>();
        }

        public IEnumerable<DeletedFeedbackByHandler> DeletedFeedbackByHandlers { get; set; }

        public int GrandTotal
        {
            get
            {
                return DeletedFeedbackByHandlers.Sum(p => p.GrandTotal);
            }
        }
    }
}