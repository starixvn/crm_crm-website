namespace CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Filters;
    using CRM.Core.Models;
    using CRM.Models;
    using CRM.Models.NotificationType;
    using CRM.Services;
    //using CRM.Services.Interfaces;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using static Fanex.NAP.Core.Business.Constant;

    [CrmAuthorize]
    [CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Read)]
    public class NotificationTypeController : Controller
    {
        private readonly INotificationTypeService notificationTypeService;
        private readonly INapSession napSession;
        private readonly INapSettings napSettings;
        private readonly IUserService userService;
        private readonly IChatBotService chatBotService;

        public NotificationTypeController(
            INotificationTypeService notificationTypeService,
            INapSession napSession,
            INapSettings napSettings,
            IChatBotService chatBotService,
            IUserService userService)
        {
            this.notificationTypeService = notificationTypeService;
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.chatBotService = chatBotService;
        }
        public async Task<IActionResult> GetSettings()
        {
            var result = new
            {
                ChatTypes = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "Group" },
                    new BaseModel { Id = 2, Name = "Personal" }
                },
                Systems = await chatBotService.GetSystems(),
                Recipients = await chatBotService.GetRecipientTypes()
            };

            return Json(result);
        }

        public IActionResult Index()
        {
           // var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationType, Policy.Host);
           //return View(isHost);
           return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetGridData([FromBody] GetGridDataParam param)
        {
            IEnumerable<NotificationType> types = await notificationTypeService.GetAsync(param);

            return Json(new { totalItems = param.TotalRows, data = types });
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Create([FromBody] CreateNotificationType model)
        {
            CommandResult commandResult = await notificationTypeService.CreateAsync(model);
            return Json(commandResult);
        }

        public async Task<IActionResult> GetById(int id)
        {
            NotificationType type = await notificationTypeService.GetByIdAsync(id);

            return Json(type);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Edit([FromBody]EditNotificationType model)
        {
            CommandResult commandResult = null;
            try {
                 commandResult = await notificationTypeService.EditAsync(model);
            }
            catch { Exception ex; }
            
            return Json(commandResult);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Delete(int id)
        {
            CommandResult commandResult = await notificationTypeService.DeleteAsync(id);
            return Json(commandResult);
        }
    }
}