// --------------------------------------------------------------------------
// <copyright file="SummaryReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.SummaryReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.SummaryReport;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// SummaryReportRepository class.
    /// </summary>
    public class SummaryReportRepository : ISummaryReportRepository
    {
        public IEnumerable<SummaryRawData> GetSummaryReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<SummaryRawData>(criteria.GetSpParams());
            }
        }
    }
}
