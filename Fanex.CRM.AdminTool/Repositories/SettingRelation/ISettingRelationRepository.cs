namespace Fanex.CRM.AdminTool.Repositories.SettingRelation
{
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.Common.Models;

    public interface ISettingRelationRepository
    {
        IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria);

        IEnumerable<SettingRelation> GetMainSettingRelations(GetMainSettingRelationsCriteria criteria);
    }
}