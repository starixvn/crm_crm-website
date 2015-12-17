// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Web.View
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.CRM.Web.View.AppStart;
    using Fanex.Logging;

    /// <summary>
    /// MVC Application.
    /// For instructions on enabling IIS6 or IIS7 classic mode, 
    /// visit http://go.microsoft.com/?LinkId=9394801.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Registers routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            var defaultParameter = new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            };

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                 defaultParameter);
        }

        /// <summary>
        /// Starts application.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError().GetBaseException();
            if (exception is HttpAntiForgeryException)
            {
                return;
            }

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                return;
            }

            string message = exception.Message +
                             "\r\nSOURCE: " + exception.Source +
                             "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "\r\nFORM: " + string.Join("~", Request.Form) +
                             "\r\nUrl: " + Request.RawUrl +
                             "\r\nQUERYSTRING: " + string.Join("~", Request.QueryString) +
                             "\r\nTARGETSITE: " + exception.TargetSite +
                             "\r\nSTACKTRACE: " + exception.StackTrace +
                             "\r\nBrowser:" + Request.Browser.Type.ToString() +
                             "\r\nBrowserVer:" + Request.Browser.Version.ToString() +
                             "\r\nAgent:" + Request.UserAgent.ToString();

            LogService.Error("CRM " + Environment.NewLine + message);
        }
    }
}