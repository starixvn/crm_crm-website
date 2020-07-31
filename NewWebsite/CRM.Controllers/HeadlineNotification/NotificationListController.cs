namespace CRM.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class NotificationListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}