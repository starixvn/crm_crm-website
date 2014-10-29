// --------------------------------------------------------------------------
// <copyright file="IFeedbackStatisticReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.10">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Models.FeedbackStatisticReport;
    using Fanex.Data.Repository;

    public interface IFeedbackStatisticReportRepository
    {
        FeedbackStatistic GetFeedbackStatistic(ICriteria criteria);

        IEnumerable<FeedbackDetail> GetFeedbackDetail(ICriteria criteria);
    }
}
