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
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Configuration;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Account Controller.
    /// </summary>
    public class AccountController : BaseController
    {
        private IUserService _userService;
        private IMaintainanceService _maintenanceService;
        private ISiteService _siteService;
        private ICurrentSession _currentSession;

        public AccountController()
            : this(null, null, null, null)
        {
        }

        public AccountController(
            IUserService userService,
            ISiteService siteService,
            ICurrentSession currentSession,
            IMaintainanceService maintenanceService)
        {
            _userService = userService ?? new UserService();
            _siteService = siteService ?? new SiteService();
            _currentSession = currentSession ?? new CurrentSession();
            _maintenanceService = maintenanceService ?? new MaintainanceService();
        }

        /// <summary>
        /// Sign in action.
        /// </summary>
        /// <returns>Sign in result.</returns>
        public ActionResult SignIn()
        {
            if (!AppSetting.IsLocalTest && (AppSession.CurrentUserMenu == null || AppSession.CurrentUser == null))
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
        /// Log out action.
        /// </summary>
        /// <returns>Log out result.</returns>
        [HttpPost]
        public JsonResult LogOut()
        {
            var result = new Result();
            try
            {
                ClearingLoginCookie();
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        public JsonResult CheckUserOnlineStatus()
        {
            try
            {
                if (AppSetting.IsLocalTest)
                {
                    return Json(new
                    {
                        ErrCode = 0
                    });
                }

                var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

                if (maintenanceInfo.IsDowntime)
                {
                    ClearingLoginCookie();
                    return Json(new
                    {
                        ErrCode = 3,
                        ErrMsg = "UM"
                    });
                }

                var siteStatus = _siteService.GetStatusById(this.SiteId);

                if (!siteStatus)
                {
                    ClearingLoginCookie();
                    return Json(new
                    {
                        ErrCode = 2,
                        ErrMsg = "Site is invalid"
                    });
                }

                UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria();
                criteria.OnUserId = Convert.ToInt32(_currentSession.OnUserId);
                _userService.ReUserCheckIn(ref criteria);

                if (criteria.IsKickedOut)
                {
                    return Json(new
                    {
                        ErrCode = 1,
                        ErrMsg = "Is kickout"
                    });
                }

                return Json(new
                {
                    OnUserId = Convert.ToInt32(_currentSession.OnUserId),
                    ErrCode = 0
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
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

        private void ClearingLoginCookie()
        {
            System.Web.HttpContext.Current.Session.Clear();
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.Now.AddDays(-1) };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
        }
        #endregion PrivateFunctions
    }
}