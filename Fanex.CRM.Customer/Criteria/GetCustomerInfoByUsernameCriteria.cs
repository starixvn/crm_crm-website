// --------------------------------------------------------------------------
// <copyright file="GetCustomerInfoByUsernameCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using Common.Enumerations;
    using Fanex.Data.Repository;
    using System;
    using System.Collections;

    /// <summary>
    /// Get Customer Information By Username Criteria.
    /// </summary>
    [Serializable]
    public class GetCustomerInfoByUsernameCriteria
    {
        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "isguest")]
        public bool IsGuest { get; set; }

        public UserRole UserRole { get; set; }

        public Hashtable SPParams()
        {
            var param = new Hashtable();
            param.Add("username", Uri.UnescapeDataString(Username));
            param.Add("isguest", IsGuest);
            param.Add("UserRole", UserRole);
            return param;
        }

        public string SettingKey()
        {
            return "Customer_CustomerInfo_GetCustomerByUsername";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
