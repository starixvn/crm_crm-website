// --------------------------------------------------------------------------
// <copyright file="SummaryReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.04.08">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Implements
{
    using Fanex.CRM.Report.Models.SummaryReport;
    using Fanex.CRM.Report.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SummaryReportRepository class.
    /// </summary>
    public class SummaryReportRepository : ISummaryReportRepository
    {
        public SummaryReportModel GetSummaryReport(ICriteria criteria)
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
