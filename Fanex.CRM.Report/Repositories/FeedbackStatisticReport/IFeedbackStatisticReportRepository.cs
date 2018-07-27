namespace Fanex.CRM.Report.Repositories.FeedbackStatisticReport
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface IFeedbackStatisticReportRepository
    {
        FeedbackStatistic GetFeedbackStatistic(ICriteria criteria);

        IEnumerable<FeedbackDetail> GetFeedbackDetail(ICriteria criteria);
    }
}
