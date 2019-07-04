// --------------------------------------------------------------------------
// <copyright file="SettingRelationRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.SettingRelation
{
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using Models.SettingManagement;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Setting Relation Repository.
    /// </summary>
    public class SettingRelationRepository : BaseRepository, ISettingRelationRepository
    {
        public IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var result = new List<Setting>();
                    while (reader.Read())
                    {
                        result.Add(new Setting
                        {
                            Id = Converter.ToInt(reader["settingid"], 0),
                            ParentId = Converter.ToInt(reader["parentid"], 0),
                            DisplayOrder = Converter.ToInt(reader["displayorder"], 0)
                        });
                    }

                    return result;
                }
            }
        }

        public IEnumerable<SettingRelation> GetMainSettingRelations(GetMainSettingRelationsCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var result = new List<SettingRelation>();
                    while (reader.Read())
                    {
                        result.Add(new SettingRelation
                        {
                            Id = Converter.ToInt(reader["settingid"], 0),
                            Value = Converter.ToString2(reader["settingvalue"]),
                            IsChild = Converter.ToInt(reader["issub"], 0) == 1,
                            DisplayOrder = Converter.ToInt(reader["displayorder"], 0)
                        });
                    }

                    return result;
                }
            }
        }

        public Result AddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
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
    }
}
