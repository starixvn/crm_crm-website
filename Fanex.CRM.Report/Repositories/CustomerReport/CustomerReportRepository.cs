namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Fanex.Common;
    using Fanex.CRM.Report.Criteria.CustomerReport;
    using Fanex.CRM.Report.Models.CustomerReport;
    using Fanex.Data;
    using System.Collections.Generic;

    public class CustomerReportRepository : ICustomerReportRepository
    {
        public IEnumerable<CustomerInfo> GetCustomerReport(GetCustomerReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var data = objectDB.Query<CustomerInfo>(criteria.GetSpParams());
                var outputParams = objectDB.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return data;
            }
        }
    }
}