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
        private readonly IChatBotService chatBotService;
        private readonly IHeadlineService headlineService;

        public NotificationListController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            IChatBotService chatBotService,
            IHeadlineService headlineService)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.chatBotService = chatBotService;
            this.headlineService = headlineService;
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
                ChatTypes = Enum.GetValues(typeof(ChatType)).Cast<ChatType>().Select(type => new BaseModel { Id = (int)type, Name = type.ToString() }),
                Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(status => status.ToString())
            };

            return Json(result);
        }

        public async Task<IActionResult> GetGridData(GetNotificationListCriteria param)
        {
            const int totalItems = 1520;
            var notifications = new List<Notification> { };
            for (var i = 0; i < totalItems; i++)
            {
                notifications.Add(new Notification
                {
                    NotificationId = i + 1,
                    MessageId = i + 1000,
                    Sender = "starixIT",
                    NotificationTypeName = "Voiding Abnormal Bet Anouncement",
                    SendDate = DateTime.Now.AddMinutes(i),
                    Title = "Void Abnormal Bet Template",
                    Content = "When users enter from the 2nd characters, the system shall list suggestions of top 10 likely recipients. Users are able to click on the suggested group headline.",
                    Status = (Status)(i % 5)
                });
            }

            return Json(new { totalItems = totalItems, data = notifications.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize)});
        }
    }
}