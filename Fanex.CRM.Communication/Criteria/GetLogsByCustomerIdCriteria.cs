// --------------------------------------------------------------------------
// <copyright file="GetLogsByCustomerIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Communication.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Communication Logs By Customer Identity Criteria.
    /// </summary>
    public class GetLogsByCustomerIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public string CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Communication_Log_GetInfoByCustomerId";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
