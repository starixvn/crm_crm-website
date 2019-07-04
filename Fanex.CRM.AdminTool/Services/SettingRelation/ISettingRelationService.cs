// --------------------------------------------------------------------------
// <copyright file="ISettingRelationService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingRelation
{
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Models.SettingManagement;
    using Models.SettingRelation;
    using System.Collections.Generic;

    public interface ISettingRelationService
    {
        IEnumerable<Setting> GetMainSettings();

        IEnumerable<Setting> GetSettingByTypes(GetSettingByTypeCriteria criteria);

        IEnumerable<MainSetting> GetMainSettingForAddRelations(GetMainSettingForAddRelationCriteria criteria);

        IEnumerable<SettingRelation> MainSettingRelations(GetMainSettingRelationsCriteria criteria);

        Result GetAddSettingRelations(AddSettingRelationsCriteria criteria);

        List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria);

        List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria);


        IEnumerable<SettingRelationItem> MainSettingRelationsCN88(GetMainSettingRelationsCriteria criteria);
    }
}
