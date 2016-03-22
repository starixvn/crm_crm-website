// --------------------------------------------------------------------------
// <copyright file="AllSettingToAddFeedback.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.07">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Feedback.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Deleted feedback model.
    /// </summary>
    [Serializable]
    public class AllSettingToAddFeedback
    {
        public IEnumerable<Setting> ExtensionList { get; set; }

        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<Setting> CommunicationToolList { get; set; }

        public IEnumerable<Setting> LanguageList { get; set; }

        public IEnumerable<Setting> MainCategoryList { get; set; }

        public IEnumerable<Setting> ParentProductList { get; set; }   

        public IEnumerable<Setting> ProductList { get; set; }

        public IEnumerable<SiteInfo> SiteList { get; set; }

        public IEnumerable<Setting> ResponseList { get; set; }

        public IEnumerable<Setting> StatusList { get; set; }

        public IEnumerable<Setting> TitleList { get; set; }

        public IEnumerable<Setting> RoleList { get; set; }

        public IEnumerable<BaseModel> LicenseeType { get; set; }
    }
}
