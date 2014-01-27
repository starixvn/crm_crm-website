// --------------------------------------------------------------------------
// <copyright file="GetCustomerInfoDetailByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria.DetailInfo
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Customer Information Detail By Identity Criteria.
    /// </summary>
    public class GetCustomerInfoDetailByIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public string CustomerId { get; set; }

        public string GetSettingKey()
        {
            return "Customer_CustomerInfoDetail_GetInfoById_New";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
