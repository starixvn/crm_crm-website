namespace CRM.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Filters;
    using CRM.Criteria.NotificationList;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using CRM.Services;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Microsoft.AspNetCore.Mvc;
    using static Fanex.NAP.Core.Business.Constant;

    [CrmAuthorize]
    [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Read)]
    public class NotificationListController : Controller
    {
        private readonly INapSession napSession;
        private readonly INapSettings napSettings;
        private readonly IUserService userService;
        private readonly INotificationListService notificationListService;
        private readonly INotificationTypeService notificationTypeService;

        public NotificationListController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            INotificationListService notificationListService,
            INotificationTypeService notificationTypeService)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.notificationListService = notificationListService;
            this.notificationTypeService = notificationTypeService;
        }

        public IActionResult Index()
        {
            var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationList, Policy.Host);
            return View(isHost);
        }

        public IActionResult GetSettings()
        {
            var result = new
            {
                NotificationTypes = Enum.GetValues(typeof(ChatType)).Cast<ChatType>().Select(type => new BaseModel { Id = (int)type, Name = type.ToString() }),
                Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(status => status.ToString())
            };

            return Json(result);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Host)]
        public async Task<IActionResult> GetGridData([FromBody]GetNotificationListCriteria param)
        {
            if (param.FromDate.HasValue && param.ToDate.HasValue)
            {
                param.FromDate = param.FromDate.Value.AddHours(0).AddMinutes(0).AddSeconds(0);
                param.ToDate = param.ToDate.Value.AddDays(1).AddMilliseconds(-1);
            }

            var notifications = (await notificationListService.GetNotificationListAsync(param)).ToList();
            return Json(new { totalItems = param.TotalRows, data = notifications });
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Host)]
        public async Task<IActionResult> EditNotificationList([FromBody]EditNotificationListCommand command)
        {
            command.UserId = napSession.UserId;
            var result = await notificationListService.EditNotificationListAsync(command);
            return Json(new { resolvedBy = napSession.UserName, result });
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Host)]
        public async Task<IActionResult> ValidateNotificationList([FromBody]ValidateNotificationListModel model)
        {
            var criteria = new ValidateNotificationListCriteria { NotificationIds = model.NotificationIds };
            var result = await notificationListService.ValidateNotificationListAsync(criteria);
            result.IsSingle = model.IsSingle;
            return Json(result);
        }
    }
}