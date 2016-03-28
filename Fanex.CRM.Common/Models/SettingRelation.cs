// --------------------------------------------------------------------------
// <copyright file="SettingRelation.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// Setting Information.
    /// </summary>
    [Serializable]
    public class SettingRelation
    {
        public int Id { get; set; }

        public string Value { get; set; }
        
        public int DisplayOrder { get; set; }

        public bool IsChild { get; set; }
    }
}
