// --------------------------------------------------------------------------
// <copyright file="SettingDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    /// <summary>
    /// Setting Data Access.
    /// </summary>
    public class SettingDataAccess : ISettingDataAccess
    {
        #region IRepository<Setting,int> Members

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Save result.</returns>
        public Result Save(Setting entity, string ip, int userId)
        {
            var result = new Result(0, string.Empty);
            try
            {
                using (var obj = new ObjectDb("Acc_CRM_SettingUpd"))
                {
                    var param = new Hashtable
                        {
                            { "userid", userId },
                            { "settingname", entity.Type },
                            { "settingid", entity.Id },
                            { "settingvalue", entity.Value },
                            { "displayorder", entity.DisplayOrder },
                            { "ip", ip },
                            { "errcode", 4 },
                            { "errmsg", 4000 }
                        };
                    obj.AssignParamValues(param);

                    obj.ExecuteNonQuery();
                    if (obj.GetParameterValue("errcode") != DBNull.Value)
                    {
                        result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                        result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                    }
                }
            }
            catch (Exception exception)
            {
                result.ErrCode = 105;
                result.ErrMsg = exception.Message;
            }

            return result;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Add result.</returns>
        public Result Add(Setting entity, string ip, int userId, int parentId)
        {
            var result = new Result(0, string.Empty);
            try
            {
                using (var obj = new ObjectDb("Acc_CRM_SettingIns"))
                {
                    var param = new Hashtable
                        {
                            { "userid", userId },
                            { "settingname", entity.Type },
                            { "settingvalue", entity.Value },
                            { "displayorder", entity.DisplayOrder },
                            { "ip", ip },
                            { "parentid", parentId },
                            { "errcode", 4 },
                            { "errmsg", 4000 }
                        };
                    obj.AssignParamValues(param);

                    obj.ExecuteNonQuery();
                    if (obj.GetParameterValue("errcode") != DBNull.Value)
                    {
                        result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                        result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                    }
                }
            }
            catch (Exception exception)
            {
                result.ErrCode = 105;
                result.ErrMsg = exception.Message;
            }

            return result;
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Remove result.</returns>
        public Result Remove(int id, string ip, int userId)
        {
            var result = new Result(0, string.Empty);
            try
            {
                using (var obj = new ObjectDb("Acc_CRM_SettingDel"))
                {
                    var param = new Hashtable
                        {
                            { "userid", userId },
                            { "settingid", id },
                            { "ip", ip },
                            { "errcode", 4 },
                            { "errmsg", 4000 }
                        };
                    obj.AssignParamValues(param);

                    obj.ExecuteNonQuery();
                    if (obj.GetParameterValue("errcode") != DBNull.Value)
                    {
                        result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                        result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                    }
                }
            }
            catch (Exception exception)
            {
                result.ErrCode = 105;
                result.ErrMsg = exception.Message;
            }

            return result;
        }

        /// <summary>
        /// Finds all settings.
        /// </summary>
        /// <returns>All settings.</returns>
        public IEnumerable<Setting> FindAll()
        {
            var allsetting = new List<Setting>();
            using (var obj = new ObjectDb("Acc_CRM_SettingSel"))
            {
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            allsetting.Add(
                                new Setting()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("SettingId")),
                                        Type = reader.GetString(reader.GetOrdinal("SettingName")),
                                        Value = reader.GetString(reader.GetOrdinal("SettingValue")),
                                        DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder"))
                                    });
                        }
                    }
                }
            }

            return allsetting;
        }

        /// <summary>
        /// Gets the setting relation.
        /// </summary>
        /// <returns>The setting relation.</returns>
        public IEnumerable<SettingRelation> GetSettingRelation()
        {
            var settingRelations = new List<SettingRelation>();

            using (var obj = new ObjectDb("Acc_CRM_SettingRelationSel"))
            {
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            settingRelations.Add(
                                new SettingRelation()
                                    {
                                        SettingId = reader.GetInt32(reader.GetOrdinal("SettingID")),
                                        ParentId = reader.GetInt32(reader.GetOrdinal("ParentId")),
                                        Order = reader["DisplayOrder"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("DisplayOrder")) : 0
                                    });
                        }
                    }
                }
            }

            return settingRelations;
        }

        #endregion

        /// <summary>
        /// Gets the setting relation by name.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>The setting relation by name.</returns>
        public IEnumerable<Setting> GetSettingRelationByName(string settingName, int parentId)
        {
            var allsetting = new List<Setting>();
            using (var obj = new ObjectDb("Setting_SettingInfo_GetRelationBySettingName"))
            {
                var param = new Hashtable { { "settingname", settingName }, { "parentid", parentId } };
                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            allsetting.Add(
                                new Setting()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("SettingId")),
                                    Value = reader.GetString(reader.GetOrdinal("SettingValue")),
                                    IsSub = reader.GetInt32(reader.GetOrdinal("IsSub")) == 1,
                                    DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? reader.GetInt32(reader.GetOrdinal("DisplayOrder")) : 0
                                });
                        }
                    }
                }
            }

            return allsetting;
        }

        /// <summary>
        /// Adds the specified setting identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Add result.</returns>
        public Result Add(string settingId, string settingName, string ip, int userId, int parentId)
        {
            var result = new Result();
            try
            {
                using (var obj = new ObjectDb("SettingRelationUpd"))
                {
                    var param = new Hashtable
                        {
                            { "userid", userId },
                            { "settingid", settingId },
                            { "settingname", settingName },
                            { "parentid", parentId },
                            { "ip", ip },
                            { "errcode", 4 },
                            { "errmsg", 4000 }
                        };

                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();

                    if (obj.GetParameterValue("errcode") != DBNull.Value)
                    {
                        result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                        result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                    }
                }
            }
            catch (Exception exception)
            {
                result.ErrCode = 105;
                result.ErrMsg = exception.Message;
            }

            return result;
        }

        /// <summary>
        /// Removes the specified setting identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Remove result.</returns>
        public Result Remove(int settingId, string ip, int userId, int parentId)
        {
            var result = new Result(0, string.Empty);

            try
            {
                using (var obj = new ObjectDb("SettingRelationDel"))
                {
                    var param = new Hashtable
                        {
                            { "userid", userId },
                            { "settingid", settingId },
                            { "parentid", parentId },
                            { "ip", ip },
                            { "errcode", 4 },
                            { "errmsg", 4000 }
                        };

                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();

                    if (obj.GetParameterValue("errcode") != DBNull.Value)
                    {
                        result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                        result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                    }
                }
            }
            catch (Exception exception)
            {
                result.ErrCode = 105;
                result.ErrMsg = exception.Message;
            }

            return result;
        }
    }
}
