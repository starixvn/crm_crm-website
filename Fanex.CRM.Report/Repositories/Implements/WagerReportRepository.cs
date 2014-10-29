// --------------------------------------------------------------------------
// <copyright file="WagerReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Implements
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.WagerReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// WagerReportRepository repository.
    /// </summary>
    public class WagerReportRepository : IWagerReportRepository
    {
        /// <summary>
        /// Gets the wager report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>WagerReport model.</returns>
        public IEnumerable<WagerRawData> GetWagerReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<WagerRawData>(criteria.GetSpParams());
            }
        }
    }
}
