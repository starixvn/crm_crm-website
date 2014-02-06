// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
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
    /// Result Model.
    /// </summary>
    [Serializable]
    public class Result
    {
        public int Code { get; set; }

        public string Message { get; set; }
    }
}
