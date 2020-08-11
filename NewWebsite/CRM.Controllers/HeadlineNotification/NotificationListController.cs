namespace CRM.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CRM.Core.Enumerables;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetSettings()
        {
            var result = new
            {
                NotificationTypes = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "Group" },
                    new BaseModel { Id = 2, Name = "Personal" }
                },
                GroupHeadlines = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "Group Headline1" },
                    new BaseModel { Id = 2, Name = "Group Headline2" },
                    new BaseModel { Id = 3, Name = "Group Headline3" },
                    new BaseModel { Id = 4, Name = "Group Headline4" }
                },
                Recipients = new List<BaseModel>
                {
                    new BaseModel { Id = 1, Name = "Recipient1" },
                    new BaseModel { Id = 2, Name = "Recipient2" },
                    new BaseModel { Id = 3, Name = "Recipient3" },
                    new BaseModel { Id = 4, Name = "Recipient4" }
                },
                Statuses = Enum.GetValues(typeof(Status)).Cast<Status>().Select(status => status.ToString())
            };

            return Json(result);
        }
        
        public IActionResult GetGridData(GetGridDataParam param)
        {
            const int totalItems = 100;
            var notifications = new List<Notification> { };
            for (var i = 0; i < totalItems; i++)
            {
                notifications.Add(new Notification
                {
                    NotificationId = i + 1,
                    MessageId = i + 1000,
                    Sender = "starixIT",
                    NotificationType = "Voiding Abnormal Bet Anouncement",
                    SendTime = DateTime.Now.AddMinutes(i),
                    GroupHeadlines = new List<BaseModel>
                    {
                        new BaseModel { Id = i, Name = $"HL0{i}"}
                    },
                    Recipients = new List<Recipient>
                    {
                        new Recipient
                        {
                            Id = i,
                            Name = $"Recipient{i}",
                            Role = new BaseModel
                            {
                                Id = i,
                                Name = i % 2 == 0 ? "Boss" : "PIC"
                            }
                        }
                    },
                    Title = "Void Abnormal Bet Template",
                    Content = "When users enter from the 2nd characters, the system shall list suggestions of top 10 likely recipients. Users are able to click on the suggested group headline.",
                    Status = (Status)(i % 5)
                });
            }

            return Json(new { totalItems = totalItems, data = notifications });
        }
    }
}