namespace CRM.Controllers
{
    using CRM.Core.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class WhoamiController : Controller
    {
        public IActionResult Index()
        {
            return Content(WhoamiHelper.GetContent(HttpContext));
        }
    }
}