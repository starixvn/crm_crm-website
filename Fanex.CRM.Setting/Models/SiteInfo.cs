namespace Fanex.CRM.Setting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    [Serializable]
    public class SiteInfo : BaseModel
    {
        public bool IsLicensee { get; set; }

        public bool IsActive { get; set; }

        public List<SiteInfo> ChildSites { get; set; }
    }
}