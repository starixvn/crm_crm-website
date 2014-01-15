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
    using Fanex.CRM.Entities;

    public interface IPermissionService
    {
        bool HasPermission(string actionPath, Dictionary<string, int> pageAccess);

        bool GetHostPermission<T>(T function);

        bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess);

        bool GetHostPermissionOnPage(string actionPath, Dictionary<string, int> pageAccess);

        List<Security> GetCustomerRoles(int custId);

        bool IsHasFeedbackReviewPermission();
    }
}
