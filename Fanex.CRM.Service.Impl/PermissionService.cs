// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

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
    /// Permission Service.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly ISecurityDataAccess _repository;

        public PermissionService()
            : this(null)
        {
        }

        public PermissionService(ISecurityDataAccess repository)
        {
            this._repository = repository ?? new SecurityDataAccess();
        }

        /// <summary>
        /// Determines whether the specified action path has permission.
        /// </summary>
        /// <param name="actionPath">The action path.</param>
        /// <param name="pageAccess">The page access.</param>
        /// <returns>Check result.</returns>
        public bool HasPermission(string actionPath, Dictionary<string, int> pageAccess)
        {
            try
            {
                if (pageAccess == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(actionPath))
                {
                    return false;
                }

                return pageAccess.ContainsKey(actionPath.ToLower());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets the host permission.
        /// </summary>
        /// <typeparam name="T">Type value.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="roleAccess">The role access.</param>
        /// <returns>Get result.</returns>
        public bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess)
        {
            try
            {
                if (roleAccess == null)
                {
                    return false;
                }

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

        /// <summary>
        /// Gets the host permission on page.
        /// </summary>
        /// <param name="actionPath">The action path.</param>
        /// <param name="pageAccess">The page access.</param>
        /// <returns>Get result.</returns>
        public bool GetHostPermissionOnPage(string actionPath, Dictionary<string, int> pageAccess)
        {
            try
            {
                if (pageAccess == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(actionPath))
                {
                    return false;
                }

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

        /// <summary>
        /// Gets the customer roles.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Get result.</returns>
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

        /// <summary>
        /// Gets the host permission.
        /// </summary>
        /// <typeparam name="T">Type value.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns>Get result.</returns>
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
