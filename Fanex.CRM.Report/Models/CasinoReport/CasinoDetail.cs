// --------------------------------------------------------------------------
// <copyright file="CasinoDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.06">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System.Collections.Generic;
    using Fanex.CRM.Report.Entities.CasinoReport;

    /// <summary>
    /// Casino Daily Detail.
    /// </summary>
    public class CasinoDetail
    {
        public string Title { get; set; }

        public IEnumerable<CasinoDetailEntity> CasinoDetails { get; set; }

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }
    }
}
