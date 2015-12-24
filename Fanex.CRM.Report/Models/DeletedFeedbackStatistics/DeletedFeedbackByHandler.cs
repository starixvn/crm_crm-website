// -----------------------------------------------------------------------
// <copyright file="DeletedFeedbackByHandler.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.DeletedFeedbackStatistics
{
    using System.Collections.Generic;

    public class DeletedFeedbackByHandler
    {
        public string Handler { get; set; }

        public IEnumerable<DeletedFeedback> DeletedFeedbacks { get; set; }
    }
}
