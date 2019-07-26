namespace Fanex.CRM.Report.Services.FeedbackStatisticReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;

    public interface IFeedbackStatisticReportService
    {
        BaseModel[] GetFeedbackStatisticFilterType();

        BaseModel[] GetFeedbackStatisticFilterTypeCN88();

        FeedbackStatistic GetFeedbackStatistic(GetFeedbackStatisticCriteria criteria);

        void ExportFeedbackStatistic(GetFeedbackStatisticCriteria criteria);

        FeedbackStatisticDetail FeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria);

        void ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria, IEnumerable<Common.Models.Setting> mainCategories);
    }
}