// --------------------------------------------------------------------------
// <copyright file="SettingInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Setting Information.
    /// </summary>
    [Serializable]
    public class Setting : BaseModel
    {
        public string Value { get; set; }

        public string SettingType { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsSubSetting { get; set; }

        public bool IsActive { get; set; }

        public int ParentId { get; set; }

        public List<Setting> ChildSettings { get; set; }
    }
}
