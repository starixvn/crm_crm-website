namespace Fanex.CRM.ActionLog.Models
{
    using System;

    [Serializable]
    public class SettingLog : ActionLog
    {
        public int Id { get; set; }

        public string Entity { get; set; }
    }
}