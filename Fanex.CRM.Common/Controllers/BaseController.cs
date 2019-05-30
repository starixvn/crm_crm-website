// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Common.Controllers
{
    using Attributes;
    using Fanex.CRM.Common.Services;
    using Fanex.NAP.Core.Business;
    using ONEbook.UM.Filters;
    using System;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [OverridableNapSupperAuth]
    [CheckUMFilter]
    public class BaseController : Controller
    {
        private readonly IPermissionService _permissionService;

        public BaseController()
        {
            _permissionService = new PermissionService();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //if (AppSession.CurrentUser != null && AppSession.CurrentUser.UserRole == UserRole.CN88Staff)
            //{
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh");
            //}
        }

        public void ChangeCulture(string lang)
        {
            Response.Cookies.Remove("Language");

            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            if (languageCookie == null) languageCookie = new HttpCookie("Language");

            languageCookie.Value = lang;

            languageCookie.Expires = DateTime.Now.AddDays(10);

            Response.SetCookie(languageCookie);

            Response.Redirect(Request.UrlReferrer.ToString());
        }

        protected IPermissionService PermissionService
        {
            get
            {
                return _permissionService;
            }
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

        public Constant.Policy GetPolicy(string functionName)
        {
            return PermissionService.GetPolicy(functionName);
        }
    }
}