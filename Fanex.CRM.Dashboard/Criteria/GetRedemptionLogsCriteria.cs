// --------------------------------------------------------------------------
// <copyright file="GetRedemptionLogsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Customer Dashboard Information Criteria.
    /// </summary>
    public class GetRedemptionLogsCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_Dashboard_GetRedemptionLogs";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
