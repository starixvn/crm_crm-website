namespace Fanex.CRM.Models.PicAndHeadline
{
    using System;

    public class ChangesLogInfo
    {
        public int LogId { get; set; }

        public string LogTime { get;set;}

        public string Name { get; set; }

        public string ActionName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string EditByName { get; set; }
    }
}