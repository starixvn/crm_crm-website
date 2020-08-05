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

        [HttpPost]
        public IActionResult GetGridData(GetGridDataParam param)
        {
            const int TotalItems = 100;
            var Types = new List<NotificationType> { };
            for (var i = 0; i < TotalItems; i++)
            {
                Types.Add(new NotificationType
                {
                    NotificationTypeId = i + 1,
                    Name = $"Notification Type {i + 1}"
                });
            }

            return Json(new { totalItems = TotalItems, data = Types });
        }
    }
}