namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class TimeInfo
    {
        public int Days { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }
}