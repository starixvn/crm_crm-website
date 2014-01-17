// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Models
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Entities;

    /// <summary>
    /// Customer Information.
    /// </summary>
    public class CustomerInfo : BaseModel
    {
        public BaseModel Title { get; set; }

        public BaseModel Site { get; set; }

        public BaseModel Country { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public List<BaseModel> Languages { get; set; }

        public List<PhoneInfo> PhoneInfo { get; set; }

        public PhoneInfo Mobile { get; set; }

        public string Email { get; set; }

        public string MSN { get; set; }

        public string Skype { get; set; }

    }
}
