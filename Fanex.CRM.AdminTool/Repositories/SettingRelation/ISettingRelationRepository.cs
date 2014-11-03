// --------------------------------------------------------------------------
// <copyright file="ISettingRelationRepository.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    public interface ISettingRelationRepository
    {
        List<SettingInfo> AllSettingRelation(GetAllSettingRelationCriteria criteria);

        List<SettingInfo> MainSettingRelations(GetMainSettingRelationsCriteria criteria);

        Result AddSettingRelations(AddSettingRelationsCriteria criteria);
    }
}
