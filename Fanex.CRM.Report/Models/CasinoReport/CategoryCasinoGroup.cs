// --------------------------------------------------------------------------
// <copyright file="CategoryCasinoGroup.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.01">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Casino Summary.
    /// </summary>
    [Serializable]
    public class CategoryCasinoGroup
    {
        public CategoryCasinoGroup(string category)
        {
            Category = category;
            CasinoGroups = new List<CasinoGroup>();
        }

        public string Category { get; set; }

        public IList<CasinoGroup> CasinoGroups { get; set; }
    }
}