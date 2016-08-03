// --------------------------------------------------------------------------
// <copyright file="IFeedbackStatisticReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.FeedbackStatisticReport
{
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.FeedbackStatisticReport;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;

    public interface IFeedbackStatisticReportService
    {
        BaseModel[] GetFeedbackStatisticFilterType();

        FeedbackStatistic GetFeedbackStatistic(GetFeedbackStatisticCriteria criteria);

        void ExportFeedbackStatistic(GetFeedbackStatisticCriteria criteria);

        FeedbackStatisticDetail FeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria);

        void ExportFeedbackStatisticDetail(GetFeedbackStatisticDetailCriteria criteria);
    }
}
