namespace Fanex.CRM.Customer.Criteria
{
    using System;
    using Common.Criteria;

    [Serializable]
    public class GetCustomerInfoByUsernameCriteria : BasedCriteria
    {
        public string Username { get; set; }
        public bool IsGuest { get; set; }

        public override string GetSettingKey() => "Customer_CustomerInfo_GetCustomerByUsername";

        public override bool IsValid() => true;
    }
}