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
        public List<SettingInfo> ExtensionList { get; set; }

        public List<CountryInfo> CountryList { get; set; }

        public List<SettingInfo> CommunicationToolList { get; set; }

        public List<SettingInfo> LanguageList { get; set; }

        public List<SettingInfo> MainCategoryList { get; set; }

        public List<SettingInfo> ProductList { get; set; }

        public List<SiteInfo> SiteList { get; set; }

        public List<SettingInfo> ResponseList { get; set; }

        public List<SettingInfo> StatusList { get; set; }

        public List<SettingInfo> TitleList { get; set; }

        public List<SettingInfo> RoleList { get; set; }

        public List<BaseModel> LicenseeType { get; set; }
    }
}
