// --------------------------------------------------------------------------
// <copyright file="SiteSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Site Setting.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SiteSetting : Setting
    {
        [DataMember]
        public bool IsLicensee { get; set; }
    }
}
