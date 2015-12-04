// -----------------------------------------------------------------------
// <copyright file="CasinoReportEnhancementRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.CasinoReportEnhancement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using Fanex.Data;

    public class CasinoReportEnhancementRepository : ICasinoReportEnhancementRepository
    {
        public IEnumerable<StatisticByDateEnhancementEntity> GetReportData(CasinoEnhancementReportCriteria criteria)
        {
            var result = new List<StatisticByDateEnhancementEntity>();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());

                result = objectDb.Query<StatisticByDateEnhancementEntity>().ToList();
            }

            return result;
        }
    }
}
