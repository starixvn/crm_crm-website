// -----------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly ISecurityDataAccess _repository;

        public PermissionService() : this(null) { }

        public PermissionService(ISecurityDataAccess repository)
        {
            this._repository = repository ?? new SecurityDataAccess();
            
        }

        public bool HasPermission(string actionPath, Dictionary<string, int> pageAccess)
        {
            try
            {
                if (pageAccess == null)
                    return false;
                if (string.IsNullOrEmpty(actionPath))
                    return false;

                return pageAccess.ContainsKey(actionPath.ToLower());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
            
        }
        
        public bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess)
        {
            try
            {
                if (roleAccess == null)
                    return false;

                string functionName = "[" + function.ToString() + "]";
                if (roleAccess.ContainsKey(functionName.ToLower()))
                {
                    return roleAccess[functionName.ToLower()] == 3;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }            
        }

        public bool GetHostPermissionOnPage(string actionPath, Dictionary<string, int> pageAccess)
        {
            try
            {
                if (pageAccess == null)
                    return false;
                if (string.IsNullOrEmpty(actionPath))
                    return false;

                if (pageAccess.ContainsKey(actionPath.ToLower()))
                {
                    return pageAccess[actionPath.ToLower()] == 3;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
            
        }

        public List<Security> GetCustomerRoles(int custId)
        {
            try
            {
                return this._repository.GetCustomerRoles(custId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(Model.PermissionService GetCustomerRoles: {0}) - {1}", custId, ex.Message));
                return new List<Security>();
            }
        }

        public bool GetHostPermission<T>(T function)
        {
            try
            {
                var functionName = "[" + function + "]";

                if (SessionExtension.RoleAccess != null)
                {
                    if (SessionExtension.RoleAccess.ContainsKey(functionName.ToLower()))
                    {
                        return SessionExtension.RoleAccess[functionName.ToLower()] == 3;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(Model.PermissionService GetHostPermission) - {0}", ex.Message));
                return false;
            }
        }

        public bool IsHasFeedbackReviewPermission()
        {
            return GetHostPermission("feedbackreview");
        }

    }
}
