
namespace Fanex.CRM.Web.Controllers.CommonFilters
{
    using System.Web.Mvc;
    using Fanex.CRM.Infrastructure.Logging;

    public class LogExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //LogExtensions.ExceptionHandler(filterContext.Exception);
        }

        private static void UnhandleException(ExceptionContext filterContext)
        {

        }
    }
}