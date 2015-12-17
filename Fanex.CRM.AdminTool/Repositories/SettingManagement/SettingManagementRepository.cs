// --------------------------------------------------------------------------
// <copyright file="SettingManagementRepository.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Setting Management Repository.
    /// </summary>
    public class SettingManagementRepository : BaseRepository, ISettingManagementRepository
    {
        public Result InsertSetting(InsertSettingCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result DeleteSetting(DeleteSettingCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result EditSetting(EditSettingCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
