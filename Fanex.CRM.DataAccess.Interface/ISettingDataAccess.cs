// --------------------------------------------------------------------------
// <copyright file="ISettingDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface ISettingDataAccess
    {
        IEnumerable<Setting> FindAll();

        IEnumerable<SettingRelation> GetSettingRelation();

        IEnumerable<Setting> GetSettingRelationByName(string settingName, int parentId);

        IList<Setting> GetStatusSetting(int type);

        Result Add(Setting entity, string ip, int userId, int parentId);

        Result Add(string settingId, string settingName, string ip, int userId, int parentId);

        Result Save(Setting entity, string ip, int userId);

        Result Remove(int id, string ip, int userId);

        Result Remove(int settingId, string ip, int userId, int parentId);
    }
}
