namespace Fanex.CRM.Report.Models.StatisticsReportByDate
{
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class StatisticsInfoByDateRepositoryResultModel
    {
        public IList<StatisticByDateEntity> RawData { get; set; }

        public IList<BaseModel> CommunicationTools { get; set; }
    }
}
