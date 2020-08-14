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

        public int NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public DateTime SendDate { get; set; }

        public string SendDateString => SendDate.ToString("MMM dd, yyyy");

        public string SendTimeString => SendDate.ToString("HH:mm:ss tt");

        public IEnumerable<Headline> Headlines { get; set; }

        public IEnumerable<Recipient> Recipients { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Status Status { get; set; }

        public string StatusString => Status.ToString();
    }
}