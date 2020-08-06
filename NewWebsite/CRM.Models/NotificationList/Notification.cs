using System;
using System.Collections.Generic;
using CRM.Core.Enumerables;

namespace CRM.Models.NotificationList
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int MessageId { get; set; }

        public string Sender { get; set; }

        public string NotificationType { get; set; }

        public DateTime SendTime { get; set; }

        public string SendDateString => SendTime.ToString("MMM dd, yyyy");

        public string SendTimeString => SendTime.ToString("HH:mm:ss tt");

        public List<BaseModel> GroupHeadlines { get; set; }

        public List<Recipient> Recipients { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Status Status { get; set; }
    }
}