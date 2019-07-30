namespace Fanex.CRM.Customer.Models
{
    using System;
    using Fanex.CRM.Common.Models;

    [Serializable]
    public class CustomerInfo : BaseCustomer
    {
        public string BirthDay { get; set; }
    }
}