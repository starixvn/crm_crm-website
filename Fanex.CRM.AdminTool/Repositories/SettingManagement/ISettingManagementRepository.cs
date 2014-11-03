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
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Infrastructure.Models;

    public interface ISettingManagementRepository
    {
        Result InsertSetting(InsertSettingCriteria criteria);

        Result DeleteSetting(DeleteSettingCriteria criteria);

        Result EditSetting(EditSettingCriteria criteria);
    }
}
