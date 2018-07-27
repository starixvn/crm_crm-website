namespace Fanex.CRM.Feedback.Models
{
    using Fanex.CRM.Common.Models;
    using System;

    [Serializable]
    public class CustomerInfo : BaseCustomer
    {
        public string BirthDay { get; set; }
    }
}
