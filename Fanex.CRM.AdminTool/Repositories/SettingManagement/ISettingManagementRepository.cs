// --------------------------------------------------------------------------
// <copyright file="ISettingManagementRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.SettingManagement
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    public interface ISettingManagementRepository
    {
        List<SettingInfo> AllSetting(GetAllSettingCriteria criteria);

        Result InsertSetting(InsertSettingCriteria criteria);
    }
}
