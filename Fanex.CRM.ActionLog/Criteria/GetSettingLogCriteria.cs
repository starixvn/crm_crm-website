// --------------------------------------------------------------------------
// <copyright file="GetSettingLogCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.02.14">Created</change>
// </history>

using System;
using Fanex.CRM.Common.Criteria;
using Fanex.Data.Repository;

namespace Fanex.CRM.ActionLog.Criteria
{
    /// <summary>
    /// Get Setting Log criteria.
    /// </summary>
    public class GetSettingLogCriteria : BasedCriteria
    {
        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int ActionId { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CRM_LogActionSettingList";
        }

        public override bool IsValid() => true;
    }
}