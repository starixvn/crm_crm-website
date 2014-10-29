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
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Communication Logs By Customer Identity Criteria.
    /// </summary>
    public class GetLogsByCustomerIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "fromdate")]
        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "todate")]
        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "statusid")]
        public int StatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Redemption_Log_GetInfoByCustomerId";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
