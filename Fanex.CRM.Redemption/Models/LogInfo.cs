// --------------------------------------------------------------------------
// <copyright file="CustomerRedemptionLogInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Redemption.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Log Information.
    /// </summary>
    [Serializable]
    public class LogInfo
    {
        public int RunningNumber { get; set; }

        public int CustomerId { get; set; }
    }
}
