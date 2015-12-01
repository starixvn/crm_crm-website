// -----------------------------------------------------------------------
// <copyright file="CasinoReportEnhancementRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Repositories.CasinoReportEnhancement
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
    using Fanex.CRM.Report.Entities.StatisticsReportByDate;
    using Fanex.CRM.Report.Models.CasinoReportEnhancement;
    using Fanex.Data;

    public class CasinoReportEnhancementRepository : ICasinoReportEnhancementRepository
    {
        public StatisticsInfoByDateEnhancementRepositoryResultModel GetReportData(CasinoEnhancementReportCriteria criteria)
        {
            var result = new StatisticsInfoByDateEnhancementRepositoryResultModel
            {
                RawData = new List<StatisticByDateEnhancementEntity>()
            };

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());
                var reader = objectDb.ExecuteReader();
                while (reader.Read())
                {
                    var reportEntity = new StatisticByDateEnhancementEntity
                    {
                        MainCategoryId = Converter.ToInt(reader["MainCategoryId"]),
                        MainCategoryName = Converter.ToString2(reader["MainCategory"]),
                        CommunicationId = Converter.ToInt(reader["CommunicationId"]),
                        CommunicationName = Converter.ToString2(reader["Communication"]),
                        Total = Converter.ToInt(reader["Total"]),
                        SubCategoryId = Converter.ToInt(reader["SubCategoryId"]),
                        SubCategoryName = Converter.ToString2(reader["SubCategory"]),
                        ProductId = Converter.ToInt(reader["ProductId"]),
                        ProductName = Converter.ToString2(reader["Product"]),
                    };

                    result.RawData.Add(reportEntity);
                }
            }

            return result;
        }
    }
}
