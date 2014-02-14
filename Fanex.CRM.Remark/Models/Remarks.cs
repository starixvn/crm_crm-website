// --------------------------------------------------------------------------
// <copyright file="Remarks.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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

        public IList<string> SpecialRemarks { get; set; }

        public IList<string> NormalRemarks { get; set; }
    }
}
