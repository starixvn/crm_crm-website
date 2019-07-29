namespace Fanex.CRM.Setting.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Common.Models;

    [Serializable]
    public class InformHeadlineSetting
    {
        public IEnumerable<BaseModel> Status { get; set; }

        public IEnumerable<BaseModel> Position { get; set; }

        public IEnumerable<BaseModel> Currency { get; set; }

        public IEnumerable<BaseModel> Site { get; set; }

        public IEnumerable<BaseModel> ProductStatus { get; set; }

        public IEnumerable<SettingRelation> Extensions { get; set; }
    }
}