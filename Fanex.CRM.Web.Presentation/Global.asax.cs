namespace Fanex.CRM.Web.View
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using Fanex.CRM.Web.View.AppStart;
    using Fanex.Logging;
    using Fanex.NAP.Core.Auth;
    using Fanex.NAP.Core.Web.Auth;

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            var defaultParameter = new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            };

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                 defaultParameter);
        }
        public override void Init()
        {
            base.Init();
            PostAuthenticateRequest += new EventHandler(MvcApplication_PostAuthenticateRequest);
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket?.Expired == false)
                {
                    var identity = new FormsIdentity(authTicket);
                    var user = new NapPrincipal(identity, AuthInfo.CreateUserData(authTicket.UserData));
                    Context.User = user;
                    Thread.CurrentPrincipal = user;
                }
            }
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Data.Configuration.DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError().GetBaseException();
            if (exception is HttpAntiForgeryException)
            {
                return;
            }

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                return;
            }

            var message = new StringBuilder(exception.Message);

            message.Append($"\r\nSOURCE:{exception.Source}");
            message.Append($"\r\nDOMAIN: {HttpContext.Current.Request.Headers["Host"]}");
            message.Append($"\r\nFORM:{string.Join("~", Request.Form)} ");
            message.Append($"\r\nUrl:{Request.RawUrl}");
            message.Append($"\r\nQUERYSTRING:{string.Join("~", Request.QueryString)}");
            message.Append($"\r\nTARGETSITE: { exception.TargetSite }");
            message.Append($"\r\nSTACKTRACE: { exception.StackTrace}");
            message.Append($"\r\nBrowser: {Request.Browser.Type}");
            message.Append($"\r\nBrowserVer:{Request.Browser.Version}");
            message.Append($"\r\nAgent:{Request.UserAgent}");

            LogService.Error(message.ToString());
        }
    }
}