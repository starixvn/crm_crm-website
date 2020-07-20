namespace Fanex.CRM.Models.PicAndHeadline
{
    using System;

    public class ChangeLogInfo
    {
        public int LogId { get; set; }

        public DateTime LogTime { get;set;}

        public string UserName { get; set; }

        public string ActionName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string EditByName { get; set; }
    }
}