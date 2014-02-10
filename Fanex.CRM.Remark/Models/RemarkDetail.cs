// --------------------------------------------------------------------------
// <copyright file="RemarkDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.10">Created</change>
// </history>

namespace Fanex.CRM.Remark.Models
{
    using System;

    /// <summary>
    /// Remark detail class.
    /// </summary>
    [Serializable]
    public class RemarkDetail
    {
        public int CustId { get; set; }

        public string Remark { get; set; }

        public bool IsSpecial { get; set; }
    }
}
