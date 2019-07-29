namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class Country : BaseModel
    {
        public int CountryCode { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}