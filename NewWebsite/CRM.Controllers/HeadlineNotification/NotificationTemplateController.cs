namespace CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Filters;
    using CRM.Core.Models;
    using CRM.Models.NotificationTemplate;
    using CRM.Models.Setting;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using static Fanex.NAP.Core.Business.Constant;

    [CrmAuthorize]
    [CrmPermission(FunctionKey = FunctionKey.NotificationTemplate, Policy = Policy.Read)]
    public class NotificationTemplateController : Controller
    {
        private readonly ILogger<NotificationTemplateController> logger;
        private readonly INapSession napSession;
        private readonly INapSettings napSettings;
        private readonly IUserService userService;
        private readonly INotificationTemplateService templateService;
        private readonly ISettingService settingService;

        public NotificationTemplateController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            INotificationTemplateService templateService,
            ISettingService settingService,
            ILogger<NotificationTemplateController> logger)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.templateService = templateService;
            this.settingService = settingService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            logger.LogInformation("notification template index - begin at {0}", DateTime.Now);
            var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationTemplate, Policy.Host);
            logger.LogInformation("notification template index - end at {0}", DateTime.Now);
            return View(isHost);
        }

        [HttpPost]
        public async Task<IActionResult> GetNotificationTemplate([FromBody]GetNotificationTemplateParam param)
        {
            logger.LogInformation("get notification template - begin at {0}", DateTime.Now);
            IEnumerable<NotificationTemplate> templates = await templateService.GetAsync(param);
            logger.LogInformation("get notification template - end at {0}", DateTime.Now);
            return Json(new { totalItems = param.TotalRows, data = templates });
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationTemplate, Policy = Policy.Host)]
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
        [CrmPermission(FunctionKey = FunctionKey.NotificationTemplate, Policy = Policy.Host)]
        public async Task<IActionResult> Edit([FromBody]EditNotificationTemplate model)
        {
            CommandResult commandResult = await templateService.EditAsync(model);
            return Json(commandResult);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationTemplate, Policy = Policy.Host)]
        public async Task<IActionResult> Delete(int id)
        {
            CommandResult commandResult = await templateService.DeleteAsync(id);
            return Json(commandResult);
        }

        public async Task<IActionResult> GetLanguages()
        {
            var param = new GetSettingParam
            {
                SettingName = SettingName.NotificationTemplateLanguage,
                IsActive = 1
            };
            IEnumerable<Setting> settings = await settingService.GetAsync(param);
            return Json(settings);
        }
    }
}