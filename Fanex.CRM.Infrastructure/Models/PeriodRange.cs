// --------------------------------------------------------------------------
// <copyright file="PeriodRange.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Models
{
    using System;

    /// <summary>
    /// Base Model.
    /// </summary>
    [Serializable]
    public class PeriodRange : BaseModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDateString { get; set; }

        public string ToDateString { get; set; }
    }
}
