namespace Fanex.CRM.AdminTool.Repositories.SettingRelation
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;
    using Fanex.Data;

    public class SettingRelationRepository : ISettingRelationRepository
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
                            Id = Converter.ToInt(reader["settingid"]),
                            ParentId = Converter.ToInt(reader["parentid"]),
                            DisplayOrder = Converter.ToInt(reader["displayorder"])
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
                            Id = Converter.ToInt(reader["settingid"]),
                            Value = Converter.ToString2(reader["settingvalue"]),
                            IsChild = Converter.ToInt(reader["issub"]) == 1,
                            DisplayOrder = Converter.ToInt(reader["displayorder"])
                        });
                    }

                    return result;
                }
            }
        }
    }
}