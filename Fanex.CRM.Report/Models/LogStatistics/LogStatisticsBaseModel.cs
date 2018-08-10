namespace Fanex.CRM.Report.Models.LogStatistics
{
    using Fanex.CRM.Infrastructure.Models;
    using System.Collections.Generic;

    public class LogStatisticsBaseModel : BaseModel
    {
        public LogStatisticsBaseModel()
        {
            ListOfCommunicationCount = new List<int>();
        }

        public IEnumerable<int> ListOfCommunicationCount { get; set; }

        public IEnumerable<int> ListOfCommunicationSumByProduct { get; set; }
    }
}
