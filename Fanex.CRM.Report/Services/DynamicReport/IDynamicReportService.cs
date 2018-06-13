// --------------------------------------------------------------------------
// <copyright file="IDynamicReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.DynamicReport
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Report.Criteria.DynamicReport;
    using Fanex.CRM.Report.Models;
    using Fanex.CRM.Setting.Models;
    using Infrastructure.Models;
    using System.Collections.Generic;

    public interface IDynamicReportService
    {
        IEnumerable<Setting> GetAllPeriodTypes();

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetAllSubProducts();

        IEnumerable<SiteInfo> GetAllSites();

        IEnumerable<BaseModel> GetAllStatuses();

        IEnumerable<SettingRelation> GetAllResponses();

        IEnumerable<SettingRelation> GetAllRoles();

        IEnumerable<Setting> GetAllCommunicationTools();

        IEnumerable<Setting> GetAllMainCategories();

        IEnumerable<SettingRelation> GetAllExtensions();

        DynamicReportInfo GetDynamicReport(GetDynamicReportCriteria criteria);

        void ExportDynamicReport(GetDynamicReportCriteria criteria, IEnumerable<Setting> mainCategories);
    }
}
