// --------------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Repositories.Implements
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories.Interfaces;
    using Fanex.Data;

    /// <summary>
    /// Setting Repository.
    /// </summary>
    public class SettingRepository : ISettingRepository
    {
        public List<SettingInfo> GetAllSetting(GetAllSettingCriteria criteria)
        {
            var allsetting = new List<SettingInfo>();
            using (var objectDB = new ObjectDb("Acc_CRM_SettingSel"))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            allsetting.Add(new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("SettingId"),
                                Type = reader.GetStringNotNull("SettingName"),
                                Value = reader.GetStringNotNull("SettingValue"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder")
                            });
                        }
                    }
                }
            }

            return allsetting;
        }
    }
}
