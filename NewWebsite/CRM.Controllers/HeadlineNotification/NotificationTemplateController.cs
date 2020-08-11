namespace CRM.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Models;
    using CRM.Models.NotificationTemplate;
    using CRM.Models.Setting;
    using CRM.Services;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationTemplateController : Controller
    {
        ////private readonly INapSession napSession;
        ////private readonly INapSettings napSettings;
        ////private readonly IUserService userService;
        private readonly INotificationTemplateService templateService;
        private readonly ISettingService settingService;

        public NotificationTemplateController(
            ////INapSession napSession,
            ////INapSettings napSettings,
            ////IUserService userService,
            INotificationTemplateService templateService,
            ISettingService settingService)
        {
            ////this.napSession = napSession;
            ////this.napSettings = napSettings;
            ////this.userService = userService;
            this.templateService = templateService;
            this.settingService = settingService;
        }

        public IActionResult Index()
        {
            ////var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationTemplate, Policy.Host);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetGridData([FromBody]GetGridDataParam param)
        {
            IEnumerable<NotificationTemplate> templates = await templateService.GetAsync(param);
            return Json(new { totalItems = param.TotalRows, data = templates });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateNotificationTemplate model)
        {
            CommandResult commandResult = await templateService.CreateAsync(model);
            return Json(commandResult);
        }

        public async Task<IActionResult> GetById(int id)
        {
            NotificationTemplate template = await templateService.GetByIdAsync(id);
            return Json(template);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]EditNotificationTemplate model)
        {
            CommandResult commandResult = await templateService.EditAsync(model);
            return Json(commandResult);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            CommandResult commandResult = await templateService.DeleteAsync(id);
            return Json(commandResult);
        }

        public async Task<IActionResult> GetLanguages()
        {
            var param = new GetSettingParam
            {
                SettingName = SettingName.ChatBotLanguage,
                IsActive = 1
            };
            IEnumerable<Setting> settings = await settingService.GetAsync(param);
            return Json(settings);
        }
    }
}