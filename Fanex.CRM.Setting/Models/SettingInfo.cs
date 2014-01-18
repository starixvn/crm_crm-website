// --------------------------------------------------------------------------
// <copyright file="SettingInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Models
{
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Setting Information.
    /// </summary>
    public class SettingInfo : BaseModel
    {
        public string Value { get; set; }

        public string Type { get; set; }

        public int DisplayOrder { get; set; }
    }
}
