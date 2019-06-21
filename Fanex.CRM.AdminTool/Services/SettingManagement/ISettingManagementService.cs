// --------------------------------------------------------------------------
// <copyright file="ISettingManagementService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingManagement
{
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using System.Collections.Generic;

    public interface ISettingManagementService
    {
        IEnumerable<Setting> GetMainSettings();

        IEnumerable<Setting> GetParentSettings();

        IEnumerable<Setting> SettingByType(GetSettingByTypeCriteria criteria);

        Result InsertSetting(InsertSettingCriteria criteria);

        Result DeleteSetting(DeleteSettingCriteria criteria);

        Setting GetSettingById(GetSettingByIdCriteria criteria);

        Result EditSetting(EditSettingCriteria criteria);

        SettingCN88 GetSettingCN88ById(GetSettingCN88ByIdCriteria criteria);

        Result EditSettingCN88(EditSettingCN88Criteria criteria);

        Result InsertSettingCN88(InsertSettingCN88Criteria criteria);
    }
}
