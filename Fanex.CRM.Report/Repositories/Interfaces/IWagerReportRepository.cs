// --------------------------------------------------------------------------
// <copyright file="IWagerReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.03.28">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Interfaces
{
    using Fanex.CRM.Report.Models.WagerReport;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// IWagerReportRepository interface.
    /// </summary>
    public interface IWagerReportRepository
    {
        WagerReportModel GetWagerReport(ICriteria criteria);
    }
}
