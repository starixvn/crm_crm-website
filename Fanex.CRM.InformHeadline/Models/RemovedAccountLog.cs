namespace Fanex.CRM.InformHeadline.Models
{
    using System;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Newtonsoft.Json;

    [Serializable]
    public class RemovedAccountLog
    {
        [JsonIgnore]
        public DateTime Date { get; set; }

        public string ClientDate => DateTimeUtilize.ChangeTimeZoneToClientDateTime(Date).ToFullFormatedDateTime();
        public string EditedBy { get; set; }
        public int Totals { get; set; }
    }
}