namespace CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CRM.Core.Constants;
    using CRM.Core.Enumerables;
    using CRM.Core.Filters;
    using CRM.Criteria.NotificationList;
    using CRM.Criteria.NotificationType;
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
        private readonly ICommonService commonService;

        public NotificationListController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            INotificationListService notificationListService,
            INotificationTypeService notificationTypeService,
            ICommonService commonService)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.notificationListService = notificationListService;
            this.notificationTypeService = notificationTypeService;
            this.commonService = commonService;
        }

        public IActionResult Index()
        {
            var isHost = userService.IsHavingPermission(napSession.UserId, napSettings.SiteId, FunctionKey.NotificationList, Policy.Host);
            return View(isHost);
        }

        public async Task<IActionResult> GetSettings()
        {
            var result = new
            {
                NotificationTypes = await notificationTypeService.GetNotificationTypeAsync(new GetNotificationTypeCriteria()),
                Statuses = Enum.GetValues(typeof(NotificationStatus)).Cast<NotificationStatus>().Select(status => status.ToString())
            };

            return Json(result);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Host)]
        public async Task<IActionResult> GetGridData([FromBody]GetNotificationListCriteria param)
        {
            if (param.FromDate.HasValue && param.ToDate.HasValue)
            {
                param.FromDate = commonService.ConvertClientToServerDateTime(param.FromDate.Value);
                param.ToDate = commonService.ConvertClientToServerDateTime(param.ToDate.Value);
            }

            var notifications = await notificationListService.GetNotificationListAsync(param);
            foreach (var notification in notifications)
            {
                notification.SendDate = commonService.ConvertToClientDateTime(notification.SendDate);
            }

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
            result.InActiveNotificationIds = new List<int> { 1, 2, 3 };
            result.ResendIds = new List<int>{ 1, 2, 3 };
            result.IsSingle = model.IsSingle;
            return Json(result);
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Host)]
        public async Task<IActionResult> ResendNotificationList([FromBody]ResendNotificationsCommand command)
        {
            command.UserId = napSession.UserId;
            var result = await notificationListService.ResendNotificationsAsync(command);
            return Json(result);
        }
    }
}