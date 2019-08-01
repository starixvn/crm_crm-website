namespace Fanex.CRM.InformHeadline.Models
{
    using System;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Helpers;
    using Newtonsoft.Json;

    [Serializable]
    public class RemovedAccountLogDetails
    {
        public string DirectAccount { get; set; }

        [JsonIgnore]
        public DateTime UpdatedTime { get; set; }

        public string ClientUpdatedTime => DateTimeUtilize.ChangeTimeZoneToClientDateTime(UpdatedTime).ToFullFormatedDateTime();
        public string Handler { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string ProductStatus { get; set; }
        public string Extension { get; set; }
    }
}