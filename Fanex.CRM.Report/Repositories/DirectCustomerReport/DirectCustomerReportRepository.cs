namespace Fanex.CRM.Report.Repositories.CustomerReport
{
    using Criteria.DirectCustomerReport;
    using Fanex.Common;
    using Fanex.Data;
    using Models.DirectCustomerReport;
    using System.Collections.Generic;

    public class DirectCustomerReportRepository : IDirectCustomerReportRepository
    {
        public IEnumerable<DirectCustomerInfo> GetReport(DirectCustomerReportCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var data = objectDB.Query<DirectCustomerInfo>(criteria.GetSpParams());
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