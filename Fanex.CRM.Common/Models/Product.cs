// --------------------------------------------------------------------------
// <copyright file="Product.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.CRM.Common.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Setting Information.
    /// </summary>
    [Serializable]
    public class Product : SettingRelation
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int ParentId { get; set; }

        public IList<Product> ChildSettings { get; set; }
    }
}
