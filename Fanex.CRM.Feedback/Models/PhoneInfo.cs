// --------------------------------------------------------------------------
// <copyright file="PhoneInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Models
{
    using System;

    /// <summary>
    /// Phone Info.
    /// </summary>
    [Serializable]
    public class PhoneInfo
    {
        public string CountryCode { get; set; }

        public string AreaCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}
