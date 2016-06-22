// --------------------------------------------------------------------------
// <copyright file="IStatisticReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis" date="2014.12.18">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.StatisticReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.StatisticReport;
    using Fanex.Data.Repository;

    public interface IStatisticReportRepository
    {
        IList<StatisticReportEntity> GetStatisticReport(ICriteria criteria);
    }
}
