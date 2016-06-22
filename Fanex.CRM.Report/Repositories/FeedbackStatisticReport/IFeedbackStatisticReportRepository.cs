// --------------------------------------------------------------------------
// <copyright file="IFeedbackStatisticReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.FeedbackStatisticReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.Data.Repository;

    public interface IFeedbackStatisticReportRepository
    {
        FeedbackStatistic GetFeedbackStatistic(ICriteria criteria);

        IEnumerable<FeedbackDetail> GetFeedbackDetail(ICriteria criteria);
    }
}
