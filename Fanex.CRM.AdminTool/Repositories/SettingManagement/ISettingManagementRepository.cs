// --------------------------------------------------------------------------
// <copyright file="ISettingManagementRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.SettingManagement
{
    using Common.Models;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Infrastructure.Models;
    using Models.SettingManagement;
    using System.Collections.Generic;

    public interface ISettingManagementRepository
    {
        Result InsertSetting(InsertSettingCriteria criteria);

        Result DeleteSetting(DeleteSettingCriteria criteria);

        Result EditSetting(EditSettingCriteria criteria);

        Result EditSettingCN88(EditSettingCN88Criteria criteria);

        SettingCN88 GetSettingCN88ById(GetSettingCN88ByIdCriteria criteria);

        Result InsertSettingCN88(InsertSettingCN88Criteria criteria);

        List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria);

        SettingItemName GetSettingName(GetSettingNameCriteria criteria);

        List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria);
    }
}
