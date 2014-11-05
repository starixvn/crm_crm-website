// --------------------------------------------------------------------------
// <copyright file="IWagerReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.WagerReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.WagerReport;
    using Fanex.Data.Repository;

    /// <summary>
    /// IWagerReportRepository interface.
    /// </summary>
    public interface IWagerReportRepository
    {
        IEnumerable<WagerRawData> GetWagerReport(ICriteria criteria);
    }
}
