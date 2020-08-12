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
        public IActionResult GetSettings()
        {
            var result = new
            {
                ChatTypes = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "Group" },
                    new BaseModel { Id = 2, Name = "Personal" }
                },
                Systems = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "Special Delete Site 1" },
                    new BaseModel { Id = 2, Name = "Special Delete Site 2" }
                },
                Recipients = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "PIC of headline 1" },
                    new BaseModel { Id = 2, Name = "PIC of headline 2" },
                }
            };

            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetGridData([FromBody] GetGridDataParam param)
        {
            IEnumerable<NotificationType> templates = await typeService.GetAsync(param);

            return Json(new { totalItems = param.TotalRows, data = templates });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNotificationType model)
        {
            CommandResult commandResult = await typeService.CreateAsync(model);
            return Json(commandResult);
        }

        public async Task<IActionResult> GetById(int id)
        {
            NotificationType type = await typeService.GetByIdAsync(id);

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