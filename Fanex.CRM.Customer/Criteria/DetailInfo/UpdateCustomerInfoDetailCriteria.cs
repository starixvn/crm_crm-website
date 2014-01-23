// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerInfoDetailCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Update Customer Information Detail Criteria.
    /// </summary>
    public class UpdateCustomerInfoDetailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "userid")]
        public string UserId { get; set; }

        public string GetSettingKey()
        {
            return "Customer_CustomerInfoDetail_Update";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
