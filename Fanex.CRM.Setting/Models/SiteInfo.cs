// --------------------------------------------------------------------------
// <copyright file="SiteInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Models
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Site Information.
    /// </summary>
    public class SiteInfo : BaseModel
    {
        public bool IsLicensee { get; set; }

        public List<SiteInfo> ChildSites { get; set; }
    }
}
