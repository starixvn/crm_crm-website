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
    using Fanex.CRM.Report.Models.WagerReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

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
        public WagerReportModel GetWagerReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader == null)
                    {
                        return null;
                    }

                    // Parse reader.
                }
            }

            return null;
        }
    }
}
