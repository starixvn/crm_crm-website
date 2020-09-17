namespace Fanex.CRM.BackgroundJob.Models
{
    using System;
    using global::CRM.Core.Extensions.XML;

    [Serializable]
    [XMLName("r")]
    public class SystemSettingItem
    {
        [XMLName("Name")]
        public string SettingName { get; set; }

        public string Value { get; set; }
    }
}