// --------------------------------------------------------------------------
// <copyright file="RedemptionLogInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Dashboard.Models
{
    using System;
    using Fanex.CRM.Setting.Models;

    /// <summary>
    /// Redemption Log Information.
    /// </summary>
    [Serializable]
    public class RedemptionLogInfo
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string Subject { get; set; }

        public string ClaimedTime { get; set; }

        public SettingInfo Status { get; set; }

        public string Remark { get; set; }
    }
}
