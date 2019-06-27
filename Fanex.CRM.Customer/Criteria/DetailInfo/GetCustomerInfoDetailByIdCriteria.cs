// --------------------------------------------------------------------------
// <copyright file="GetCustomerInfoDetailByIdCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria.DetailInfo
{
    using Common.Criteria;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Customer Information Detail By Identity Criteria.
    /// </summary>
    public class GetCustomerInfoDetailByIdCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "custid")]
        public string CustomerId { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_CustomerInfoDetail_GetInfoById_New";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
