// --------------------------------------------------------------------------
// <copyright file="GetCustomerInfoByUserName.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    /// <summary>
    /// Get Customer Info.
    /// </summary>
    public class GetCustomerInfoByUserNameCriteria
    {
        public string UserName { get; set; }

        public string IsGuest { get; set; }

        public string GetSettingKey()
        {
            return "Gaming_BingoOutStanding_GetReport";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
