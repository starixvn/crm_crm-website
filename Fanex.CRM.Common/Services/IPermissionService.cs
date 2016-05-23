// --------------------------------------------------------------------------
// <copyright file="IPermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Common.Services
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using NAP.Core.Models.ValueObject;

    public interface IPermissionService
    {
        bool HasReadPermission(string functionName);

        bool HasHostPermission(string functionName);

        bool HasPermission(string functionName, Constant.Policy policy);

        Constant.Policy GetPolicy(string functionName);
    }
}
