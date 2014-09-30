// --------------------------------------------------------------------------
// <copyright file="SettingRelationRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.SettingRelation
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Setting Relation Repository.
    /// </summary>
    public class SettingRelationRepository : ISettingRelationRepository
    {
        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All setting.</returns>
        public List<SettingInfo> AllSetting(GetAllSettingCriteria criteria)
        {
            List<SettingInfo> result = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SettingInfo
                    {
                        Id = Converter.ToInt(reader["settingid"], 0),
                        Type = Converter.ToString2(reader["settingname"]),
                        Value = Converter.ToString2(reader["settingvalue"]),
                        DisplayOrder = Converter.ToInt(reader["displayorder"], 0)
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all setting relation.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All setting relation.</returns>
        public List<SettingInfo> AllSettingRelation(GetAllSettingRelationCriteria criteria)
        {
            List<SettingInfo> result = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SettingInfo
                    {
                        Id = Converter.ToInt(reader["settingid"], 0),
                        ParentId = Converter.ToInt(reader["parentid"], 0),
                        DisplayOrder = Converter.ToInt(reader["displayorder"], 0)
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the main setting relations.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main setting relations.</returns>
        public List<SettingInfo> MainSettingRelations(GetMainSettingRelationsCriteria criteria)
        {
            List<SettingInfo> result = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SettingInfo
                    {
                        Id = Converter.ToInt(reader["settingid"], 0),
                        Value = Converter.ToString2(reader["settingvalue"]),
                        IsSubSetting = Converter.ToInt(reader["issub"], 0) == 1,
                        DisplayOrder = Converter.ToInt(reader["displayorder"], 0)
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Adds the setting relations.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Add result.</returns>
        public Result AddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            Result addResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                addResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return addResult;
        }
    }
}
