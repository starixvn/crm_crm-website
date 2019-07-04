// --------------------------------------------------------------------------
// <copyright file="ISettingRelationRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.SettingRelation
{
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Models.SettingManagement;
    using System.Collections.Generic;

    public interface ISettingRelationRepository
    {
        IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria);

        IEnumerable<SettingRelation> GetMainSettingRelations(GetMainSettingRelationsCriteria criteria);

        Result AddSettingRelations(AddSettingRelationsCriteria criteria);

        List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria);

        List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria);
    }
}
