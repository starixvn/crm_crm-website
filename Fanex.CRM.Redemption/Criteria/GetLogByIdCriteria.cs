// --------------------------------------------------------------------------
// <copyright file="GetLogByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Log By Identify Criteria.
    /// </summary>
    public class GetLogByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "redemptionid")]
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
