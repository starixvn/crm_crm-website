
namespace Fanex.CRM.Infrastructure.Common
{
    using System.Configuration;
    using DNAide.Core;

    public static class AppSetting
    {       
        public static string Version {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["version"]); }
        }
        public static bool IsLocalTest {
            get { return (CastAide.AsString(ConfigurationManager.AppSettings["isLocalTest"]) == "1"); }
        }
        public static int ServerId
        {
            get { return (CastAide.AsInt32(ConfigurationManager.AppSettings["serverid"])); }
        }

        public static string AccountingUrlDefault
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["accdefaultpage"]); }
        }

        public static string TestAccount
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["testaccount"]); }
        }


        public static int TimeSpan
        {
            get { return CastAide.AsInt32(ConfigurationManager.AppSettings["timeSpan"]); }
        }
    }
}
