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
    using Models.SettingManagement;
    using System.Collections.Generic;
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
                return result ?? new SettingCN88();
            }
        }

        public List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = objectDB.Query<MainSetting>(criteria.GetSpParams());
                return result.ToList();
            }
        }

        public List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = objectDB.Query<SettingItemCN88>(criteria.GetSpParams());
                return result.ToList();
            }
        }

        public SettingItemName GetSettingName(GetSettingNameCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var result = objectDB.Query<SettingItemName>(criteria.GetSpParams()).FirstOrDefault();
                return result ?? new SettingItemName();
            }
        }

        public Result InsertSettingCN88(InsertSettingCN88Criteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }
    }
}
