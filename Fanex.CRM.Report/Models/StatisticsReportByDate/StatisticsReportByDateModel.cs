namespace Fanex.CRM.Report.Models.StatisticsReportByDate
{
    using Fanex.CRM.Infrastructure.Models;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class StatisticsReportByDateModel : CategoryIssuesInfo
    {
        public string AliasName { get; set; }

        public IList<BaseModel> CommunicationTools { get; set; }
    }
}
