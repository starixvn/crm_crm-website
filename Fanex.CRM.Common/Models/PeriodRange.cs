namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class PeriodRange : BaseModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDateString { get; set; }

        public string ToDateString { get; set; }
    }
}