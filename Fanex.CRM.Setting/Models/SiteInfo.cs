// --------------------------------------------------------------------------
// <copyright file="SiteInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Site Information.
    /// </summary>
    [Serializable]
    public class SiteInfo : BaseModel
    {
        public bool IsLicensee { get; set; }

        public bool IsActive { get; set; }

        public List<SiteInfo> ChildSites { get; set; }
    }
}
