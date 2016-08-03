// --------------------------------------------------------------------------
// <copyright file="GetActionLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

namespace Fanex.CRM.ActionLog.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get ActionLog criteria.
    /// </summary>
    public abstract class GetActionLogCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int ActionId { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; }
    }
}
