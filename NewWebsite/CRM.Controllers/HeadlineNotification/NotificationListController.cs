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
        private readonly INotificationListService notificationListService;

        public NotificationListController(
            INapSession napSession,
            INapSettings napSettings,
            IUserService userService,
            INotificationListService notificationListService)
        {
            this.napSession = napSession;
            this.napSettings = napSettings;
            this.userService = userService;
            this.notificationListService = notificationListService;
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
            var notifications = (await notificationListService.GetNotificationList(param)).ToList();
            var fakeData = new List<Notification>
            {
                new Notification
                {
                    NotificationId = 1,
                    MessageId = 1001,
                    Sender = "starixIT",
                    NotificationTypeId = 2,
                    NotificationTypeName = "Voiding Abnornal Bet Announcement",
                    SendDate = DateTime.Now,
                    Headlines = new List<Headline>
                    {
                        new Headline
                        {
                            NotificationId = 1,
                            HeadlineId = 1,
                            HeadlineName = "Headline 1"
                        },
                        new Headline
                        {
                            NotificationId = 1,
                            HeadlineId = 2,
                            HeadlineName = "Headline 2"
                        }
                    },
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            NotificationId = 1,
                            RecipientId = 1,
                            RecipientName = "Recipient 1",
                            RoleId = 1,
                            RoleName = "Role 1"
                        },
                        new Recipient
                        {
                            NotificationId = 1,
                            RecipientId = 2,
                            RecipientName = "Recipient 2",
                            RoleId = 2,
                            RoleName = "Role 2"
                        }
                    },
                    Title = "Voiding Abnornal Bet Template",
                    Content = "Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template",
                    Status = "Sent"
                },

                new Notification
                {
                    NotificationId = 2,
                    MessageId = 1002,
                    Sender = "starixIT",
                    NotificationTypeId = 1,
                    NotificationTypeName = "Voiding Abnornal Bet Announcement",
                    SendDate = DateTime.Now,
                    Headlines = new List<Headline>
                    {
                        new Headline
                        {
                            NotificationId = 1,
                            HeadlineId = 1,
                            HeadlineName = "Headline 1"
                        },
                        new Headline
                        {
                            NotificationId = 1,
                            HeadlineId = 2,
                            HeadlineName = "Headline 2"
                        }
                    },
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            NotificationId = 1,
                            RecipientId = 1,
                            RecipientName = "Recipient 1",
                            RoleId = 1,
                            RoleName = "Role 1"
                        },
                        new Recipient
                        {
                            NotificationId = 1,
                            RecipientId = 2,
                            RecipientName = "Recipient 2",
                            RoleId = 2,
                            RoleName = "Role 2"
                        }
                    },
                    Title = "Voiding Abnornal Bet Template",
                    Content = "Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template Voiding Abnornal Bet Template",
                    Status = "Error",
                    Reason = "Something made it wrong"
                }
            };

            notifications.AddRange(fakeData);
            return Json(new { totalItems = param.TotalRows, data = notifications });
        }

        [HttpPost]
        [CrmPermission(FunctionKey = FunctionKey.NotificationList, Policy = Policy.Host)]
        public async Task<IActionResult> EditNotificationList(EditNotificationListCommand command)
        {
            command.UserId = napSession.UserId;
            var result = await notificationListService.EditNotificationList(command);
            return Json(result);
        }
    }
}