namespace Fanex.CRM.Web.View
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Web.View.AppStart;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.Sentry.RavenClient;
    using Fanex.NAP.Core.Auth;
    using Fanex.NAP.Core.Web.Auth;
    using Newtonsoft.Json;
    using SharpRaven;

    public class MvcApplication : HttpApplication
    {
        private static string RedisApplicationName => ConfigurationManager.AppSettings["RedisApplicationName"];
        private static string RedisConnectionString => ConfigurationManager.AppSettings["RedisConnectionString"];

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
            PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
        }

        private void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
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

            LogManager
                   .SetDefaultLogCategory(AppSetting.SentryDNS)
                   .Use(new SentryLogging(new SentryEngineOptions
                   {
                       Dsn = new Dsn(AppSetting.SentryDNS)
                   }));

            ExceptionHandler
                .UseConfig(new LoggingConfiguration())
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

            CacheServiceManager.Use(() => new RedisCache(new RedisCacheOptions
            {
                ConnectionString = RedisConnectionString,
                ApplicationName = RedisApplicationName
            }));

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var errMessage = "Message: " + exception.Message + Environment.NewLine +
                            "DATABASE INFO:" + Environment.NewLine +
                             JsonConvert.SerializeObject(DatabaseInfoGetters.GetDbExceptionInfo(exception), Formatting.Indented);
            var logMessage = new LogMessage(errMessage)
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();
            Logger.Log.Error(logMessage);
        }
    }
}