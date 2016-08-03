// --------------------------------------------------------------------------
// <copyright file="SettingRelation.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Common.Models
{
    using System;

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
