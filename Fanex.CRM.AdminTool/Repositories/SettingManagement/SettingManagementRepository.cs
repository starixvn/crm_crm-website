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
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using Fanex.Data;

    /// <summary>
    /// Setting Management Repository.
    /// </summary>
    public class SettingManagementRepository : ISettingManagementRepository
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
        /// Inserts setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertSetting(InsertSettingCriteria criteria)
        {
            Result insertResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                insertResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return insertResult;
        }

        /// <summary>
        /// Deletes the setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Delete result.</returns>
        public Result DeleteSetting(DeleteSettingCriteria criteria)
        {
            Result deleteResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                deleteResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return deleteResult;
        }

        /// <summary>
        /// Edits the setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Edit result.</returns>
        public Result EditSetting(EditSettingCriteria criteria)
        {
            Result editResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                editResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return editResult;
        }
    }
}
