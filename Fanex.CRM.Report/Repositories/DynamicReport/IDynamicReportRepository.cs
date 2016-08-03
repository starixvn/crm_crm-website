// --------------------------------------------------------------------------
// <copyright file="IDynamicReportRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.DynamicReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;

    public interface IDynamicReportRepository
    {
        IEnumerable<DynamicReportInfo> GetDynamicReport(GetDynamicReportCriteria criteria);
    }
}
