namespace CRM.Controllers
{
    using CRM.Models.NotificationType;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    public class NotificationTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}