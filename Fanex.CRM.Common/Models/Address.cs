// --------------------------------------------------------------------------
// <copyright file="AddressInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Address Information.
    /// </summary>
    [Serializable]
    public class Address
    {
        public List<string> LineList { get; set; }

        public Country Country { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostCode { get; set; }
    }
}
