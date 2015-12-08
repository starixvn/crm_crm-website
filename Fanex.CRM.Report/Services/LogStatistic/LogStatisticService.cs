// -----------------------------------------------------------------------
// <copyright file="LogStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.LogStatistic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.LogStatistic;
    using Fanex.CRM.Report.Entities.LogStatistic;
    using Fanex.CRM.Report.Helpers;
    using Fanex.CRM.Report.Models.LogStatistic;
    using Fanex.CRM.Report.Repositories.LogStatistic;
    using Fanex.CRM.Report.Resources;

    public class LogStatisticService : ILogStatisticService
    {
        private readonly ILogStatisticRepository _logStatisticRepository;

        public LogStatisticService()
            : this(null)
        {
        }

        public LogStatisticService(ILogStatisticRepository logStatisticRepository)
        {
            _logStatisticRepository = logStatisticRepository ?? new LogStatisticRepository();
        }

        public LogStatisticViewModel GetReportData(LogStatisticCriteria criteria)
        {
            try
            {
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

                var repositoryResult = _logStatisticRepository.GetReportData(criteria);

                var reportTitle = string.Format(
                    MainReportTitle(),
                    ReportHelper.GetLogStatisticSiteTypeName(criteria.SiteType),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());

                var logStatistic = new LogStatisticViewModel
                {
                    Id = 1,
                    Name = reportTitle,
                    CommunicationColumns = GetCommunicationColumns(repositoryResult),
                    ProductColumns = GetProductColumns(repositoryResult)
                };

                int productLength = logStatistic.ProductColumns.Count();
                int communicationLength = logStatistic.CommunicationColumns.Count();

                logStatistic.MainCategories = GetMainCategories(repositoryResult, productLength, communicationLength);
                SumTotalCommunicationForMainCategory(logStatistic.MainCategories);

                return logStatistic;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public virtual string MainReportTitle()
        {
            return Labels.LogStatisticsReport_Title;
        }

        private IEnumerable<LogStatisticMainCategory> GetMainCategories(
            IEnumerable<LogStatisticEntity> repositoryData, int productLength, int communicationLength)
        {
            var dataGroupByMainCategory = repositoryData.GroupBy(data =>
                     new
                     {
                         MainCategoryId = data.MainCategoryId,
                         MainCategory = data.MainCategory
                     });

            return dataGroupByMainCategory.Select(reportData => new LogStatisticMainCategory
            {
                Id = reportData.Key.MainCategoryId,
                Name = reportData.Key.MainCategory,
                SubCategories = GetSubCategories(reportData),
                ListOfCommunicationSumByProduct = Enumerable.Repeat(0, productLength).ToList(),
                ListOfCommunicationCount = Enumerable.Repeat(0, productLength * communicationLength).ToList()
            }).ToList();
        }

        private IEnumerable<LogStatisticBaseModel> GetSubCategories(
            IGrouping<dynamic, LogStatisticEntity> mainCategoryGroup)
        {
            var dataGroupBySubCategory = mainCategoryGroup.GroupBy(s => new { s.SubCategoryId, s.SubCategory });

            return dataGroupBySubCategory.Select(data => new LogStatisticBaseModel
            {
                Id = data.Key.SubCategoryId,
                Name = data.Key.SubCategory,
                ListOfCommunicationSumByProduct = GetSumOfCommunicationByProductForSubCategory(data),
                ListOfCommunicationCount = GetNumberOfCommunicationForSubCategory(data)
            }).ToList();
        }

        private List<int> GetSumOfCommunicationByProductForSubCategory(
            IGrouping<dynamic, LogStatisticEntity> subCategoryGroup)
        {
            var communicatonToolGroup = subCategoryGroup.GroupBy(data =>
                    new
                    {
                        ProductId = data.ProductId,
                    }).ToList();

            return communicatonToolGroup.ToList().Select(data => data.Sum(p => p.Total)).ToList();
        }

        private List<int> GetNumberOfCommunicationForSubCategory(
            IGrouping<dynamic, LogStatisticEntity> subCategoryGroup)
        {
            List<int> numberOfCommunication = new List<int>();

            var communicatonToolGroup = subCategoryGroup.GroupBy(data =>
                    new
                    {
                        ProductId = data.ProductId,
                    }).ToList();

            foreach (var data in communicatonToolGroup)
            {
                numberOfCommunication.AddRange(data.Select(p => p.Total));
            }

            return numberOfCommunication;
        }

        private void SumTotalCommunicationForMainCategory(IEnumerable<LogStatisticMainCategory> mainCategories)
        {
            foreach (var mainCategory in mainCategories)
            {
                foreach (var subCategory in mainCategory.SubCategories)
                {
                    SumCommunicationCountForMainCategory(mainCategory, subCategory.ListOfCommunicationCount);
                    SumCommunicationCountByProductForMainCategory(mainCategory, subCategory.ListOfCommunicationSumByProduct);
                }
            }
        }

        private void SumCommunicationCountForMainCategory(
            LogStatisticBaseModel mainCategory, List<int> numBerOfCommunicationBySubCategory)
        {
            for (int i = 0; i < numBerOfCommunicationBySubCategory.Count; i++)
            {
                mainCategory.ListOfCommunicationCount[i] += numBerOfCommunicationBySubCategory[i];
            }
        }

        private void SumCommunicationCountByProductForMainCategory(
            LogStatisticBaseModel mainCategory, List<int> sumOfCommunicationBySubCategory)
        {
            for (int i = 0; i < sumOfCommunicationBySubCategory.Count; i++)
            {
                mainCategory.ListOfCommunicationSumByProduct[i] += sumOfCommunicationBySubCategory[i];
            }
        }

        private List<BaseModel> GetCommunicationColumns(
            IEnumerable<LogStatisticEntity> repositoryData)
        {
            var communicatonToolGroup = repositoryData.GroupBy(data =>
                    new
                    {
                        Id = data.CommunicationId,
                        Name = data.Communication
                    }).ToList();

            return communicatonToolGroup.Select(item => new BaseModel
            {
                Id = item.Key.Id,
                Name = item.Key.Name
            }).ToList();
        }

        private List<BaseModel> GetProductColumns(
            IEnumerable<LogStatisticEntity> repositoryData)
        {
            var productGroup = repositoryData.GroupBy(data =>
                    new
                    {
                        Id = data.ProductId,
                        Name = data.Product
                    }).ToList();

            return productGroup.Select(item => new BaseModel
            {
                Id = item.Key.Id,
                Name = item.Key.Name
            }).ToList();
        }
    }
}