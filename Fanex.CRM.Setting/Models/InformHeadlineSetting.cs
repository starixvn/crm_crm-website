// --------------------------------------------------------------------------
// <copyright file="InformHeadlineSetting.cs" company="">
// Copyright (c) . All rights reserved.
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
        public IEnumerable<BaseModel> Status { get; set; }

        public IEnumerable<BaseModel> Position { get; set; }

        public IEnumerable<BaseModel> Currency { get; set; }

        public IEnumerable<BaseModel> Site { get; set; }

        public IEnumerable<BaseModel> ProductStatus { get; set; }

        public IEnumerable<SettingRelation> Extensions { get; set; }
    }
}
