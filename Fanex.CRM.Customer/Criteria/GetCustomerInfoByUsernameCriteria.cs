namespace Fanex.CRM.Customer.Criteria
{
    using System;
    using System.Collections;
    using Common.Criteria;
    using Fanex.Data.Repository;

    [Serializable]
    public class GetCustomerInfoByUsernameCriteria : BasedCriteria
    {
        [SpParam(SqlParamName = "username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "isguest")]
        public bool IsGuest { get; set; }

        public Hashtable SPParams()
        {
            var param = new Hashtable();
            param.Add("username", Uri.UnescapeDataString(Username));
            param.Add("isguest", IsGuest);
            param.Add("UserRole", UserRole);
            return param;
        }

        public string SettingKey()
        {
            return "Customer_CustomerInfo_GetCustomerByUsername";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}