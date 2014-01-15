// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Web.Controllers.Helper;

    /// <summary>
    /// Base Controller.
    /// </summary>
    public class BaseController : Controller
    {
        private readonly IPermissionService _permissionService;

        private readonly IUserService _userService;

        public BaseController()
            : this(null, null)
        {
        }

        public BaseController(IPermissionService permissionService, IUserService userService)
        {
            _permissionService = permissionService ?? new PermissionService();
            _userService = userService ?? new UserService();
        }
        
        protected ActionResult ForceLogin()
        {
            return Redirect(Fanex.CRM.Infrastructure.Common.AppSetting.AccountingUrlDefault);
        }

        #region PrivateFunctions

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AppSetting.IsLocalTest)
            {
                GetSessionForLocalTest();
            }
            else
            {
                var isOnline = _userService.CheckUserOnlineStatus();
                if (isOnline.ErrCode != 0)
                {
                    ForceLogin();
                }
            }

            if (!WebConfigApplicationSettings.NotAuthenticateAction.Contains(filterContext.ActionDescriptor.ActionName.ToLower()))
            {
                if (AppSession.RoleAccess == null ||
                    AppSession.PageAccess == null ||
                    AppSession.CurrentUser == null)
                {
                    System.Web.HttpContext.Current.Session.Clear();
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.Now.AddDays(-1) };
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    FormsAuthentication.SignOut();
                    filterContext.Result = ForceLogin();
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private static void AccessDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Common",
                action = "AccessDenied"
            }));
        }

        /// <summary>
        /// Gets the session for local test.
        /// </summary>
        private void GetSessionForLocalTest()
        {
            var userName = AppSetting.TestAccount;
            var sid = string.Empty;

            var user = new User();
            IUserService accountService = new UserService();
            user = accountService.GetUseInfo(userName);
            AppSession.CurrentUser = user;
            if (user == null)
            {
                ForceLogin();
            }

            if (!string.IsNullOrEmpty(sid) &&
                !string.IsNullOrEmpty(AppSession.UserSessionId) &&
                !sid.Equals(AppSession.UserSessionId))
            {
                System.Web.HttpContext.Current.Session.Clear();
            }

            if (!string.IsNullOrEmpty(sid))
            {
                AppSession.UserSessionId = sid;
            }

            FormsAuthentication.SetAuthCookie(user.Username, false);
            if (AppSession.PageAccess == null || AppSession.RoleAccess == null)
            {
                var userRoleAccess = new Dictionary<string, int>();
                var userPageAccess = new Dictionary<string, int>();

                _userService.AssignRoles(user.Id, ref userRoleAccess, ref userPageAccess);
                AppSession.PageAccess = userPageAccess;
                AppSession.RoleAccess = userRoleAccess;
            }

            AppSession.UserMenu = _userService.GetUserMenu(user.Id);
        }

        #endregion PrivateFunctions
    }
}