namespace Fanex.CRM.InformHeadline.Models
{
    using System;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Newtonsoft.Json;

    [Serializable]
    public class RemovedAccountLog
    {
        public int LogId { get; set; }

        [JsonIgnore]
        public DateTime LogDate { get; set; }

        public string ClientLogDate => DateTimeUtilize.ChangeTimeZoneToClientDateTime(LogDate).ToFullFormatedDateTime();
        public string Username { get; set; }
        public int Totals { get; set; }
    }
}