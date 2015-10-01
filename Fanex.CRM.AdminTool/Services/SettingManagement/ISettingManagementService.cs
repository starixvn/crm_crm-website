// --------------------------------------------------------------------------
// <copyright file="ISettingManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ISettingManagementService
    {
        IEnumerable<SettingInfo> GetMainSettings();

        IEnumerable<SettingInfo> GetParentSettings();

        IEnumerable<SettingInfo> SettingByType(GetSettingByTypeCriteria criteria);

        Result InsertSetting(InsertSettingCriteria criteria);

        Result DeleteSetting(DeleteSettingCriteria criteria);

        SettingInfo GetSettingById(GetSettingByIdCriteria criteria);

        Result EditSetting(EditSettingCriteria criteria);
    }
}
