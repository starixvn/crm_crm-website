// --------------------------------------------------------------------------
// <copyright file="IPermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Model.ValueObject;

    public interface IPermissionService
    {
        Dictionary<string, int> GetRoleAccess(IEnumerable<FunctionPolicy> currentFunctions);

        bool HasReadPermission(string functionName);

        bool HasHostPermission(string functionName);

        bool HasPermission(string functionName);
    }
}
