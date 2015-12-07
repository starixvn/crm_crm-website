// -----------------------------------------------------------------------
// <copyright file="LogStatisticRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.LogStatistic
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Report.Criteria.LogStatistic;
    using Fanex.CRM.Report.Entities.LogStatistic;
    using Fanex.Data;

    public class LogStatisticRepository : ILogStatisticRepository
    {
        public IEnumerable<LogStatisticEntity> GetReportData(LogStatisticCriteria criteria)
        {
            var result = new List<LogStatisticEntity>();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());

                result = objectDb.Query<LogStatisticEntity>().ToList();
            }

            return result;
        }
    }
}
