namespace Fanex.CRM.DataAccess.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    public class SettingDataAccess : ISettingDataAccess
    {

        #region IRepository<Setting,int> Members

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

        public Result Add(Setting entity, string Ip, int userId, int parentId)
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
                            { "ip", Ip },
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

        public Result Remove(int Id, string Ip, int userId)
        {
            var result = new Result(0, string.Empty);
            try
            {
                using (var obj = new ObjectDb("Acc_CRM_SettingDel"))
                {
                    var param = new Hashtable
                        {
                            { "userid", userId },
                            { "settingid", Id },
                            { "ip", Ip },
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


        public IEnumerable<Setting> GetSettingRelationByName(string settingName, int parentId)
        {
            var allsetting = new List<Setting>();
            using (var obj = new ObjectDb("SettingGetRelationBySettingName"))
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
