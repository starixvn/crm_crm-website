namespace Fanex.CRM.Common.Helpers
{
    using System.Web;

    public static class IPHelper
    {
        public static string ClientIP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }
    }
}