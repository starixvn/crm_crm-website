// --------------------------------------------------------------------------
// <copyright file="GetUplineInfoOfUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Customer Information By Username Criteria.
    /// </summary>
    public class GetUplineInfoOfUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfo_GetUplineList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
