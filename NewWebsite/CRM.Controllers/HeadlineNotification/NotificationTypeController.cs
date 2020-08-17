namespace CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Enumerables;
    using CRM.Core.Models;
    using CRM.Models;
    using CRM.Models.NotificationType;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using static Fanex.NAP.Core.Business.Constant;

    //[CrmAuthorize]
    //[CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Read)]
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
                ChatTypes = Enum.GetValues(typeof(ChatType)).Cast<ChatType>().Select(type => new BaseModel { Id = (int)type, Name = type.ToString() }),
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
            IEnumerable<NotificationType> types = await notificationTypeService.GetNotificationTypeAsync(param);

            return Json(new { totalItems = param.TotalRows, data = types });
        }

        [HttpPost]
        //[CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Create([FromBody] CreateNotificationType model)
        {
            CommandResult commandResult = await notificationTypeService.CreateAsync(model);
            return Json(commandResult);
        }

        [HttpPost]
        //[CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Edit([FromBody]EditNotificationType model)
        {
            CommandResult commandResult = await notificationTypeService.EditAsync(model);
            
            return Json(commandResult);
        }

        [HttpPost]
        //[CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Delete(int id)
        {
            CommandResult commandResult = await notificationTypeService.DeleteAsync(id);
            return Json(commandResult);
        }
    }
}