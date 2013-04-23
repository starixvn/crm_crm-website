namespace Fanex.CRM.DataAccess.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using DNAide.Core;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    public class UserDataAccess : IUserDataAccess
    {

        public User GetUesrInfo(string userName)
        {
            var user = new User();

            using (ObjectDb obj = new ObjectDb("UserInfoSel"))
            {
                var param = new Hashtable { { "username", userName }, { "userid", 0 }, { "roleid", 0 } };
                obj.AssignParamValues(param);
                obj.ExecuteNonQuery();
                user.Username = userName;
                user.Id = CastAide.AsInt32(obj.GetParameterValue("userid"));
                user.RoleId = CastAide.AsInt32(obj.GetParameterValue("roleid"));
                user.IsAllowUpdateReview = false;
            }
            return user;
        }

        public IEnumerable<Menu> GetMenu(int userId)
        {
            var menus = new List<Menu>();
            using (var obj = new ObjectDb("MenuSelByCustID"))
            {
                var param = new Hashtable { { "custid", userId }, { "system", "CRM" } };
                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var menu = new Menu
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("MenuId")),
                                Name = reader.IsDBNull(reader.GetOrdinal("Menu"))
                                           ? string.Empty
                                           : reader.GetString(reader.GetOrdinal("Menu")),
                                Url = reader.IsDBNull(reader.GetOrdinal("url"))
                                          ? string.Empty
                                          : reader.GetString(reader.GetOrdinal("url")).Replace(".aspx", ""),
                                Order = reader.IsDBNull(reader.GetOrdinal("Order"))
                                            ? 0
                                            : reader.GetInt32(reader.GetOrdinal("Order"))
                            };

                            var parentId = reader.IsDBNull(reader.GetOrdinal("parentid"))
                                             ? 0
                                             : reader.GetInt32(reader.GetOrdinal("parentid"));

                            if (parentId > 0)
                            {
                                menu.Parent = new Menu { Id = parentId };
                            }

                            menus.Add(menu);
                        }
                    }
                }
            }

            return menus;
        }

        public bool OnlineUserUpdate(int onlineUserId)
        {
            bool online = false;
                        
            try
            {                
                using (var obj = new ObjectDb("UserOnlineUpdate"))
                {
                    var param = new Hashtable { { "on_userid", onlineUserId }, { "iskickedout", 0 } };
                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();
                    online = !CastAide.AsBoolean(obj.GetParameterValue("iskickedout"));
                }
            }
            catch
            {
                online = false;
            }
            return online;
        }

    }
}
