// --------------------------------------------------------------------------
// <copyright file="SettlementReportEntity.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Entities.SettlementReport
{
    using System;

    /// <summary>
    /// Settlement Report Entity.
    /// </summary>
    [Serializable]
    public class SettlementReportEntity
    {
        public int Month { get; set; }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public int NumberOfIssues { get; set; }
    }
}
