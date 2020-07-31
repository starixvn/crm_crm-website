namespace CRM.Controllers
{
    using System.Collections.Generic;
    using CRM.Models.NotificationTemplate;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationTemplateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetGridData(GetGridDataParam param)
        {
            const int TotalItems = 100;
            var templates = new List<NotificationTemplate> { };
            for (var i = 0; i < TotalItems; i++)
            {
                templates.Add(new NotificationTemplate
                {
                    NotificationTemplateId = i + 1,
                    Name = $"Notification Template {i + 1}"
                });
            }

            return Json(new { totalItems = TotalItems, data = templates });
        }
    }
}