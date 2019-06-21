// --------------------------------------------------------------------------
// <copyright file="SettingManagementRepository.cs" company="">
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
    using Data;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Models;
    using System.Linq;

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

        public Result EditSettingCN88(EditSettingCN88Criteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public SettingCN88 GetSettingCN88ById(GetSettingCN88ByIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = objectDB.Query<SettingCN88>(criteria.GetSpParams()).FirstOrDefault();
                return result;
            }
        }
    }
}
