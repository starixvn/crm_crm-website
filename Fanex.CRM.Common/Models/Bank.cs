namespace Fanex.CRM.Common.Models
{
    using System;

    [Serializable]
    public class Bank : BaseModel
    {
        public string Address { get; set; }

        public string SwiftCode { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string HolderName { get; set; }

        public Address AddressInfo { get; set; }
    }
}