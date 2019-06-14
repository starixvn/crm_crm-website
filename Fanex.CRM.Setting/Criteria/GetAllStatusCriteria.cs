// --------------------------------------------------------------------------
// <copyright file="GetAllStatusCriteria.cs" company="">
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
    /// Get All Language Criteria.
    /// </summary>
    public class GetAllStatusCriteria : BasedLanguageCriteria
    {
        public GetAllStatusCriteria(int statusType)
        {
            ViewType = statusType;
        }

        [SpParam(SqlParamName = "type")]
        public int ViewType { get; set; }

        public override string GetSettingKey()
        {
            return "Setting_SettingInfo_GetAllStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
