// --------------------------------------------------------------------------
// <copyright file="UserInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Models
{
    using System;
    using Fanex.CRM.Infrastructure.Models;

    /// <summary>
    /// User Information.
    /// </summary>
    [Serializable]
    public class UserInfo : BaseModel
    {
        public int RoleId { get; set; }

        public string Password { get; set; }
    }
}
