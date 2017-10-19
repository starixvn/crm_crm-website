using System.Collections.Generic;

namespace Fanex.CRM.Report.Models.DirectCustomerReport
{
    public class DirectCustomerReportInfo
    {
        public IList<DirectCustomerInfo> CustomerList { get; set; }

        public int TotalCustomer { get; set; }
    }
}
