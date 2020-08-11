namespace CRM.Controllers.HeadlineNotification
{
    using System.Collections.Generic;
    using CRM.Models;
    using Microsoft.AspNetCore.Mvc;

    public class ChatTypeMappingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSettings()
        {
            var chatType = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Group" },
                new BaseModel { Id = 2, Name = "Personal" }
            };

            var bots = new List<BaseModel>
            {
                new BaseModel { Id = 100020, Name = "247_Notification" },
            };

            var lang = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "EN" },
                new BaseModel { Id = 2, Name = "SC" },
                new BaseModel { Id = 3, Name = "EN & SC" },
            };

            var status = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Not Mapped" },
                new BaseModel { Id = 2, Name = "Mapped" },
                new BaseModel { Id = 3, Name = "Bot Removed" },
            };

            var result = new
            {
                ChatTypes = chatType,
                MappingStatuses = status,
                Languages = lang,
                Bots = bots
            };
            return Json(result);
        }
    }
}