// --------------------------------------------------------------------------
// <copyright file="CustomerDefaultSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.06.30">Created</change>
// </history>

namespace Fanex.CRM.Communication.Models
{
    /// <summary>
    /// Customer Default Setting.
    /// </summary>
    public class CustomerDefaultSetting
    {
        public int CustId { get; set; }

        public int CountryId { get; set; }

        public int SiteId { get; set; }

        public int RoleId { get; set; }

        public int DefaultLanguage { get; set; }

        public int LicenseeTypeId { get; set; }
    }
}
