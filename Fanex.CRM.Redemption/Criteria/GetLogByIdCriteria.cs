// --------------------------------------------------------------------------
// <copyright file="GetLogByIdCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Criteria
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Log By Identify Criteria.
    /// </summary>
    public class GetLogByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "id")]
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "Redemption_Log_GetInfoById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
