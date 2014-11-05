// --------------------------------------------------------------------------
// <copyright file="InformHeadlineSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.09.22">Created</change>
// </history>

namespace Fanex.CRM.Setting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Inform Headline Setting.
    /// </summary>
    [Serializable]
    public class InformHeadlineSetting
    {
        public List<BaseModel> Status { get; set; }

        public List<BaseModel> ProductStatus { get; set; }

        public List<SettingInfo> Extensions { get; set; }
    }
}
