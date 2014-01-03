// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using DNAide.Core;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Service.Impl;
    using Fanex.CRM.Service.Interface;
    using Fanex.CRM.Web.Controllers.Helper;

    /// <summary>
    /// Account Controller.
    /// </summary>
    public class AccountController : BaseController
    {
        private IUserService _accountService;
        private IPermissionService _permission;

        public AccountController()
            : this(null, null)
        {
        }

        public AccountController(IUserService service, IPermissionService permission)
        {
            _accountService = service ?? new UserService();
            _permission = permission ?? new PermissionService();
        }

        /// <summary>
        /// Sign in action.
        /// </summary>
        /// <returns>Sign in result.</returns>
        public ActionResult SignIn()
        {
            if (!AppSetting.IsLocalTest && (AppSession.RoleAccess == null || AppSession.PageAccess == null || AppSession.CurrentUser == null))
            {
                ForceLogin();
            }
            else
            {
                return new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }

            return View();
        }

        /// <summary>
        /// Login from Accounting.
        /// </summary>
        /// <param name="loginInfo">The login information.</param>
        /// <returns>Login Result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult DirectLogin(string loginInfo)
        {
            try
            {
                System.Web.HttpContext.Current.Session.Clear();

                var userName = string.Empty;
                var sid = string.Empty;
                var userOnlineId = 0;

                var keyPath = Request.PhysicalApplicationPath + "/IbcKeys/ibckeydec.snk";
                RSACryptoPad.EncryptionThread encryption = new RSACryptoPad.EncryptionThread();

                var ps = Request["params"];
                ps = ps.Replace(' ', '+');

                var encryptValue = encryption.DecryptString(ps, 1024, keyPath);
                var arrValue = encryptValue.Split('&');
                if (!string.IsNullOrEmpty(encryptValue) &&
                    arrValue.Length > 0)
                {
                    userName = arrValue[1];
                    userOnlineId = CastAide.AsInt32(arrValue[2]);
                    sid = arrValue[3];
                    if (userOnlineId > 0)
                    {
                        SessionExtension.OnlineUserId = CastAide.AsInt32(userOnlineId);
                    }
                    
                    var isOnline = _accountService.CheckUserOnlineStatus();
                    if (isOnline.ErrCode != 0)
                    {
                        SendMailNotice(string.Format("Login Fail: {0} ({1})", isOnline.ErrMsg, DateTime.Now));
                        return ForceLogin();
                    }
                }
                else
                {
                    SendMailNotice(string.Format("Login Fail: Login key does not valid ({0})", DateTime.Now));
                    return ForceLogin();
                }

                var user = new User();
                user = GetUserInfo(userName);
                if (user == null)
                {
                    SendMailNotice(string.Format("Login Fail: User info does not valid ({0})", DateTime.Now));
                    return ForceLogin();
                }

                if (!string.IsNullOrEmpty(sid))
                {
                    AppSession.UserSessionId = sid;
                }

                FormsAuthentication.SetAuthCookie(user.Username, false);
                if (AppSession.PageAccess == null ||
                    AppSession.RoleAccess == null ||
                    AppSession.CurrentUser == null)
                {
                    var userRoleAccess = new Dictionary<string, int>();
                    var userPageAccess = new Dictionary<string, int>();

                    _accountService.AssignRoles(user.Id, ref userRoleAccess, ref userPageAccess);
                    AppSession.PageAccess = userPageAccess;
                    AppSession.RoleAccess = userRoleAccess;
                }

                AppSession.UserMenu = _accountService.GetUserMenu(user.Id);
                return new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Index"
                }));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return ForceLogin();
            }
        }

        /// <summary>
        /// Log out action.
        /// </summary>
        /// <returns>Log out result.</returns>
        [HttpPost]
        public JsonResult LogOut()
        {
            var result = new Result();
            try
            {
                System.Web.HttpContext.Current.Session.Clear();
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                FormsAuthentication.SignOut();
                result.ErrMsg = FormsAuthentication.LoginUrl;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                result.ErrCode = 111;
                result.ErrMsg = exception.Message;
            }

            JsonResult jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return jsonResult;
        }

        /// <summary>
        /// Checks the user online status.
        /// </summary>
        /// <returns>Checking result.</returns>
        [HttpPost]
        public JsonResult CheckUserOnlineStatus()
        {
            var result = new Result();

            try
            {
                result = _accountService.CheckUserOnlineStatus();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrCode = (int)Enumerations.ErrorCode.SystemError;
                result.ErrMsg = ex.Message;
            }

            if (result.ErrCode != 0)
            {
                System.Web.HttpContext.Current.Session.Clear();
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.Now.AddDays(-1) };
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                FormsAuthentication.SignOut();
            }

            JsonResult jsonResult = Json(result);
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        #region PrivateFunctions

        /// <summary>
        /// Sends mail the notice.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SendMailNotice(string message)
        {
            var mailSetting = MailSection.Instance[MailSection.Setting.Send]["Notice"].Mails["crmlogin"];
            SendMailExtension.AccSendMail(
                MailSection.HostName.Gmail,
                mailSetting.To,
                mailSetting.Cc,
                mailSetting.Bcc,
                "CRM - User Login",
                "CRM - User Login Notice",
                message,
                new MailAttachment());
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The user information.</returns>
        private User GetUserInfo(string userName)
        {
            var user = new User();
            user = _accountService.GetUseInfo(userName);
            user.IsAllowUpdateReview = _permission.GetHostPermission(Enumerations.Functions.Feedback.FeedbackReview, AppSession.RoleAccess);

            if (user != null)
            {
                AppSession.CurrentUser = user;
            }

            return user;
        }

        #endregion PrivateFunctions
    }
}