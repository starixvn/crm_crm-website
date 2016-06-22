// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CustomerReport
{
    using System;
    using Fanex.CRM.Common.Models;

    /// <summary>
    /// Customer Information.
    /// </summary>
    [Serializable]
    public class CustomerInfo : BaseCustomer
    {
        public DateTime BirthDay { get; set; }

        public string Username { get; set; }

        public string PIC1
        {
            get
            {
                return PersonalInfoContacts[0].ToString();
            }
        }

        public string PIC2
        {
            get
            {
                return PersonalInfoContacts[1].ToString();
            }
        }

        public string PIC3
        {
            get
            {
                return PersonalInfoContacts[2].ToString();
            }
        }

        public string PIC4
        {
            get
            {
                return PersonalInfoContacts[3].ToString();
            }
        }
    }
}
