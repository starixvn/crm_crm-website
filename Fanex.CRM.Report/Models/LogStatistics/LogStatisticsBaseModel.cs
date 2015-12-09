// -----------------------------------------------------------------------
// <copyright file="LogStatisticBaseModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.LogStatistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Models;

    public class LogStatisticsBaseModel : BaseModel
    {
        public LogStatisticsBaseModel()
        {
            ListOfCommunicationCount = new List<int>();
        }

        public List<int> ListOfCommunicationCount { get; set; }

        public List<int> ListOfCommunicationSumByProduct { get; set; }

        public int TotalCommunications
        {
            get
            {
                return ListOfCommunicationCount.Sum();
            }
        }
    }
}
