namespace CRM.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Models;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationTypeController : Controller
    {
        private readonly INotificationTypeService typeService;

        public NotificationTypeController(
            INotificationTypeService typeService)
        {
            this.typeService = typeService;
        }
        
        public IActionResult GetRecipientTypes()
        {
            var result = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "PIC of headline" }
            };

            return Json(result);
        }
        public IActionResult GetChatTypes()
        {
            var result = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Group" },
                new BaseModel { Id = 2, Name = "Personal" }
            };

            return Json(result);
        }

        public IActionResult GetSystems()
        {
            var result = new List<BaseModel>
            {
                new BaseModel { Id = 1, Name = "Special Delete Site" }
            };

            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetGridData(GetGridDataParam param)
        {
            const int TotalItems = 100;
            var types = new List<NotificationType> { };
            for (var i = 0; i < TotalItems; i++)
            {
                types.Add(new NotificationType
                {
                    NotificationTypeId = i + 1,
                    Name = $"Notification Type {i + 1}"
                });
            }

            return Json(new { totalItems = TotalItems, data = types });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateNotificationType model)
        {
            CommandResult commandResult = await typeService.CreateAsync(model);
            return Json(commandResult);
        }

        public IActionResult GetById(int id)
        {
            var type = new NotificationType
            {
                NotificationTypeId = id,
                Name = "Notification Type 1"
            };
            return Json(type);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]EditNotificationType model)
        {
            CommandResult commandResult = await typeService.EditAsync(model);
            return Json(commandResult);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CommandResult commandResult = await typeService.DeleteAsync(id);
            return Json(commandResult);
        }
    }
}