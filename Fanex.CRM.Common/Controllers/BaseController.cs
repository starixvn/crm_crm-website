// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Common.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BaseController : Controller
    {
        private readonly ICurrentContext _currentContext;

        private readonly ICurrentSession _currentSession;

        private readonly IServiceContext _serviceContext;

        private readonly string _environment = NapSettings.Settings.Environment;

        private readonly int _siteId = NapSettings.Settings.SiteId;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        private IPermissionService _permissionService = null;

        public BaseController()
        {
            this._currentContext = new CurrentContext();
            ViewBag.Session = this._currentSession = new CurrentSession();
            this._serviceContext = new ServiceContext
            {
                UserId = this._currentSession.UserId,
                SiteId = this._siteId,
                Ip = this._currentContext.IP
            };

            ViewFlags = new ViewFlags
            {
                IsLive = this._environment.Equals(Constant.LIVE),
                HasEditRight = false
            };
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
                ViewBag.ViewFlags = value;
            }
        }

        protected IPermissionService PermissionService
        {
            get
            {
                _permissionService = _permissionService ?? new PermissionService();
                return _permissionService;
            }
        }

        protected ICurrentContext CurrentContext
        { 
            get { return _currentContext; }
        }

        protected ICurrentSession CurrentSession
        {
            get { return _currentSession; }
        }

        protected IServiceContext ServiceContext
        {
            get { return _serviceContext; }
        }

        protected string Environment
        {
            get { return _environment; }
        }

        protected int SiteId
        {
            get { return _siteId; }
        }

        /// <summary>
        /// GetRollAccess: get permission to access with each menu item of current user login.
        /// </summary>
        /// <param name="currentFunctions">The current functions.</param>
        /// <returns>
        /// Roll Access.
        /// </returns>
        public Dictionary<string, int> GetRoleAccess(IEnumerable<FunctionPolicy> currentFunctions)
        {
            return PermissionService.GetRoleAccess(currentFunctions);
        }

        /// <summary>
        /// HasPermission: check that user has permission to access one menu link.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>
        /// Has Read Permission indicator.
        /// </returns>
        public bool HasReadPermission(string functionName)
        {
            return PermissionService.HasReadPermission(functionName);
        }

        public bool HasHostPermission(string functionName)
        {
            return PermissionService.HasHostPermission(functionName);
        }

        public bool HasPermission(string functionName, Constant.Policy policy)
        {
            return PermissionService.HasPermission(functionName, policy);
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
        }
    }
}
