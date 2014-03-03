// --------------------------------------------------------------------------
// <copyright file="AddressInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CustomerReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Address Information.
    /// </summary>
    [Serializable]
    public class AddressInfo
    {
        public List<string> LineList { get; set; }

        public CountryInfo Country { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostCode { get; set; }
    }
}
