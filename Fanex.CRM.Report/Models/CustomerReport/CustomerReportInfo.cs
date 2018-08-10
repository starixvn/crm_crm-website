namespace Fanex.CRM.Report.Models.CustomerReport
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CustomerReportInfo
    {
        public IList<CustomerInfo> CustomerList { get; set; }

        public int TotalCustomer { get; set; }
    }
}
