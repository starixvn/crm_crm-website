namespace CRM.Controllers
{
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Filters;
    using CRM.Core.Models;
    using CRM.Criteria.NotificationType;
    using CRM.Models.NotificationType;
    using CRM.Services;
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
        private readonly ICommonService commonService;

        public NotificationTypeController(
            INotificationTypeService notificationTypeService,
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            ICommonService commonService)
        {
            this.notificationTypeService = notificationTypeService;
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.commonService = commonService;
        }

        public async Task<IActionResult> GetSettings()
        {
            var result = new
            {
                ChatTypes = commonService.GetChatTypes(),
                Systems = await commonService.GetSystemAsync(),
                RecipientTypes = await commonService.GetRecipientTypeAsync()
            };

            return Json(result);
        }

        public IActionResult Index()
        {
            var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationType, Policy.Host);
            return View(isHost);
        }

        [HttpPost]
        public async Task<IActionResult> GetNotificationType([FromBody] GetNotificationTypeParam param)
        {
            var criteria = new GetNotificationTypeCriteria
            {
                RecipientTypeIds = param.RecipientTypeIds,
                ChatTypeIds = param.ChatTypeIds,
                SystemId = param.SystemId,
                NotificationTypeName = param.NotificationTypeName,
                PageIndex = param.PageIndex,
                PageSize = param.PageSize
            };

            var notificationTypes = await notificationTypeService.GetNotificationTypeAsync(criteria);

            return Json(new { totalItems = criteria.TotalRows, data = notificationTypes });
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Create([FromBody] CreateNotificationType model)
        {
            CommandResult commandResult = await notificationTypeService.CreateAsync(model);
            return Json(commandResult);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationType, Policy = Policy.Host)]
        public async Task<IActionResult> Edit([FromBody] EditNotificationType model)
        {
            CommandResult commandResult = await notificationTypeService.EditAsync(model);
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