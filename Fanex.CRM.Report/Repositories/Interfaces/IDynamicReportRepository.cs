// --------------------------------------------------------------------------
// <copyright file="IDynamicReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;

    public interface IDynamicReportRepository
    {
        List<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria);
    }
}
