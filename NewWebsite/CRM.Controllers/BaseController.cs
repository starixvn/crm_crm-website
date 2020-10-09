namespace CRM.Controllers
{
    using CRM.Core.Filters;
    using Microsoft.AspNetCore.Mvc;

    [CrmAuthorize]
    public class BaseController : Controller
    {
        protected string ClientIP => HttpContext.Connection.RemoteIpAddress.ToString();
    }
}