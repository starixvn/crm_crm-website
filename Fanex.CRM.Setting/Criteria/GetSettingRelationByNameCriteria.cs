// --------------------------------------------------------------------------
// <copyright file="GetSettingRelationByNameCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Criteria
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Setting Relation By Name Criteria.
    /// </summary>
    public class GetSettingRelationByNameCriteria : BasedLanguageCriteria
    {
        [SpParam(SqlParamName = "settingname")]
        public string SettingName { get; set; }

        [SpParam(SqlParamName = "parentid")]
        public int ParentId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_SettingGetRelationBySettingName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
