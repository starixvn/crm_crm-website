// --------------------------------------------------------------------------
// <copyright file="Remarks.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Remark detail class.
    /// </summary>
    [Serializable]
    public class Remarks
    {
        public int CustId { get; set; }

        public IEnumerable<string> SpecialRemarks { get; set; }

        public IEnumerable<string> NormalRemarks { get; set; }
    }
}
