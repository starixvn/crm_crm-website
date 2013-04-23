// -----------------------------------------------------------------------
// <copyright file="IPermissionService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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
