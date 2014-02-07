// --------------------------------------------------------------------------
// <copyright file="GetLogsByCustomerIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Communication Logs By Customer Identity Criteria.
    /// </summary>
    public class GetLogsByCustomerIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public override string GetSettingKey()
        {
            return "Redemption_ViewLog_GetInfoByCustomerId";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
