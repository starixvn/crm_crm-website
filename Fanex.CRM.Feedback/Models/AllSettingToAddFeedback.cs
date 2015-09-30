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
        public IEnumerable<SettingInfo> ExtensionList { get; set; }

        public IEnumerable<CountryInfo> CountryList { get; set; }

        public IEnumerable<SettingInfo> CommunicationToolList { get; set; }

        public IEnumerable<SettingInfo> LanguageList { get; set; }

        public IEnumerable<SettingInfo> MainCategoryList { get; set; }

        public IEnumerable<SettingInfo> ProductList { get; set; }

        public IEnumerable<SiteInfo> SiteList { get; set; }

        public IEnumerable<SettingInfo> ResponseList { get; set; }

        public IEnumerable<SettingInfo> StatusList { get; set; }

        public IEnumerable<SettingInfo> TitleList { get; set; }

        public IEnumerable<SettingInfo> RoleList { get; set; }

        public IEnumerable<BaseModel> LicenseeType { get; set; }
    }
}
