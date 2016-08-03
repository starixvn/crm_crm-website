// --------------------------------------------------------------------------
// <copyright file="CustomerLogInfo.cs" company="">
// Copyright (c) . All rights reserved.
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
    /// Customer Log Information.
    /// </summary>
    [Serializable]
    public class CustomerLogInfo
    {
        public int CustomerId { get; set; }

        public IEnumerable<LogInfo> Logs { get; set; }
    }
}
