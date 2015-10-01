// --------------------------------------------------------------------------
// <copyright file="ISettingRelationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingRelation
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    public interface ISettingRelationService
    {
        IEnumerable<SettingInfo> GetMainSettings();

        IEnumerable<SettingInfo> GetSettingByTypes(GetSettingByTypeCriteria criteria);

        IEnumerable<SettingInfo> GetMainSettingForAddRelations(GetMainSettingForAddRelationCriteria criteria);

        IEnumerable<SettingInfo> MainSettingRelations(GetMainSettingRelationsCriteria criteria);

        Result GetAddSettingRelations(AddSettingRelationsCriteria criteria);
    }
}
