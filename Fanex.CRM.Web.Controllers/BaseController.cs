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
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Service.Impl;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Base Controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly ICurrentContext CurrentContext;

        protected readonly ICurrentSession CurrentSession;

        protected readonly IServiceContext ServiceContext;

        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        private ViewFlags _viewFlags;

        private MenuService _menuService;

        public BaseController()
        {
            this.CurrentContext = new CurrentContext();
            ViewBag.Session = this.CurrentSession = new CurrentSession();
            this.ServiceContext = new ServiceContext
            {
                UserId = this.CurrentSession.UserId,
                SiteId = this.SiteId,
                Ip = this.CurrentContext.IP
            };

            ViewFlags = new ViewFlags
            {
                IsLive = this.Environment.Equals(Constant.LIVE),
                HasEditRight = false
            };
            this._menuService = new MenuService();
        }

        /// <summary>
        /// Gets a value indicating whether [is site login].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [is site login]; otherwise, <c>false</c>.
        /// </value>
        public bool IsSiteLogin
        {
            get
            {
                return this._issiteLogin;
            }
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = this._viewFlags = value;
            }
        }

        /// <summary>
        /// Execute before each request, if Downtime then go to Downtime page
        /// Or not Authenticate will go to Login Page.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AppSession.CurrentActionName = filterContext.ActionDescriptor.ActionName;
            base.OnActionExecuting(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            AppSession.CurrentHttpMethod = null;
            // Get current working on menu for getting display name,... on webpage
            if (filterContext.Result is ViewResult && string.Equals(AppSession.CurrentActionName, "Index", StringComparison.OrdinalIgnoreCase))
            {
                var menuPath = MenuService.GetMenuPathFromRequestFilePath(filterContext.HttpContext.Request.FilePath, Request.ApplicationPath);
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                AppSession.CurrentWorkingOnMenu = this._menuService.GetMenuByURL(menuPath);
            }
        }
    }
}