namespace CRM.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Models;
    using CRM.Models.NotificationTemplate;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationTemplateController : Controller
    {
        ////private readonly INapSession napSession;
        ////private readonly INapSettings napSettings;
        ////private readonly IUserService userService;
        private readonly INotificationTemplateService templateService;

        public NotificationTemplateController(
            ////INapSession napSession,
            ////INapSettings napSettings,
            ////IUserService userService,
            INotificationTemplateService templateService)
        {
            ////this.napSession = napSession;
            ////this.napSettings = napSettings;
            ////this.userService = userService;
            this.templateService = templateService;
        }

        public IActionResult Index()
        {
            ////var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationTemplate, Policy.Host);
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateNotificationTemplate model)
        {
            CommandResult commandResult = await templateService.CreateAsync(model);
            return Json(commandResult);
        }
    }
}