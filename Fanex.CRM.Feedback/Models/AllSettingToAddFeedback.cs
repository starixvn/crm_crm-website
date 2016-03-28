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
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Deleted feedback model.
    /// </summary>
    [Serializable]
    public class AllSettingToAddFeedback
    {
        public IEnumerable<SettingRelation> ExtensionList { get; set; }

        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<Setting> CommunicationToolList { get; set; }

        public IEnumerable<Setting> LanguageList { get; set; }

        public IEnumerable<Setting> MainCategoryList { get; set; }

        public IEnumerable<Product> ProductList { get; set; }

        public IEnumerable<Product> SubProductList { get; set; }

        public IEnumerable<SiteInfo> SiteList { get; set; }

        public IEnumerable<SettingRelation> ResponseList { get; set; }

        public IEnumerable<Setting> StatusList { get; set; }

        public IEnumerable<SettingRelation> TitleList { get; set; }

        public IEnumerable<SettingRelation> RoleList { get; set; }

        public IEnumerable<BaseModel> LicenseeType { get; set; }
    }
}