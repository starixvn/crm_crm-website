// -----------------------------------------------------------------------
// <copyright file="LogStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.LogStatistics
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Report.Criteria.LogStatistics;
    using Fanex.CRM.Report.Entities.LogStatistics;
    using Fanex.CRM.Report.Helpers;
    using Fanex.CRM.Report.Models.LogStatistics;
    using Fanex.CRM.Report.Models.StatisticsReportByDate;
    using Fanex.CRM.Report.Repositories.LogStatistics;
    using Fanex.CRM.Report.Resources;

    public class LogStatisticsService : ILogStatisticsService
    {
        private readonly ILogStatisticsRepository _logStatisticsRepository;

        public LogStatisticsService()
            : this(null)
        {
        }

        public LogStatisticsService(ILogStatisticsRepository logStatisticsRepository)
        {
            _logStatisticsRepository = logStatisticsRepository ?? new LogStatisticsRepository();
        }

        public LogStatisticsViewModel GetReportData(LogStatisticsCriteria criteria)
        {
            var reportTitle = string.Format(
               Labels.LogStatisticsReport_Title,
               ReportHelper.GetLogStatisticSiteTypeName(criteria.SiteType),
               DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
               DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());

            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            var repositoryResult = _logStatisticsRepository.GetReportData(criteria);

            var logStatistic = new LogStatisticsViewModel
            {
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

        #region GetReportData private methods

        private IEnumerable<LogStatisticsMainCategory> GetMainCategories(
            IEnumerable<LogStatisticsEntity> repositoryData, int productLength, int communicationLength)
        {
            var dataGroupByMainCategory = repositoryData.GroupBy(data => data.MainCategoryId);

            return dataGroupByMainCategory.Select(reportData => new LogStatisticsMainCategory
            {
                Id = reportData.Key,
                Name = reportData.First().MainCategory,
                SubCategories = GetSubCategories(reportData),
                ListOfCommunicationSumByProduct = Enumerable.Repeat(0, productLength).ToList(),
                ListOfCommunicationCount = Enumerable.Repeat(0, productLength * communicationLength).ToList()
            }).ToList();
        }

        private IEnumerable<LogStatisticsBaseModel> GetSubCategories(
            IGrouping<int, LogStatisticsEntity> mainCategoryGroup)
        {
            var dataGroupBySubCategory = mainCategoryGroup.GroupBy(s => s.SubCategoryId);

            return dataGroupBySubCategory.Select(data => new LogStatisticsBaseModel
            {
                Id = data.Key,
                Name = data.First().SubCategory,
                ListOfCommunicationSumByProduct = GetSumOfCommunicationByProductForSubCategory(data),
                ListOfCommunicationCount = GetNumberOfCommunicationForSubCategory(data)
            }).ToList();
        }

        private List<int> GetSumOfCommunicationByProductForSubCategory(
            IGrouping<int, LogStatisticsEntity> subCategoryGroup)
        {
            var communicatonToolGroup = subCategoryGroup.GroupBy(data => data.ProductId).ToList();

            return communicatonToolGroup.Select(data => data.Sum(p => p.Total)).ToList();
        }

        private List<int> GetNumberOfCommunicationForSubCategory(
            IGrouping<int, LogStatisticsEntity> subCategoryGroup)
        {
            List<int> numberOfCommunication = new List<int>();

            var communicatonToolGroup = subCategoryGroup.GroupBy(data => data.ProductId).ToList();

            foreach (var data in communicatonToolGroup)
            {
                numberOfCommunication.AddRange(data.Select(p => p.Total));
            }

            return numberOfCommunication;
        }

        private void SumTotalCommunicationForMainCategory(IEnumerable<LogStatisticsMainCategory> mainCategories)
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
            LogStatisticsBaseModel mainCategory, IEnumerable<int> numBerOfCommunicationBySubCategory)
        {
            var totalNumBerOfCommunicationBySubCategory = numBerOfCommunicationBySubCategory.Count();
            var listOfCommunicationCount = mainCategory.ListOfCommunicationCount.ToList();
            for (int i = 0; i < totalNumBerOfCommunicationBySubCategory; i++)
            {
                listOfCommunicationCount[i] += numBerOfCommunicationBySubCategory.ElementAt(i);
            }

            mainCategory.ListOfCommunicationCount = listOfCommunicationCount;
        }

        private void SumCommunicationCountByProductForMainCategory(
            LogStatisticsBaseModel mainCategory, IEnumerable<int> sumOfCommunicationBySubCategory)
        {
            var totalSumOfCommunicationBySubCategory = sumOfCommunicationBySubCategory.Count();
            var listOfCommunicationCount = mainCategory.ListOfCommunicationSumByProduct.ToList();
            for (int i = 0; i < totalSumOfCommunicationBySubCategory; i++)
            {
                listOfCommunicationCount[i] += sumOfCommunicationBySubCategory.ElementAt(i);
            }

            mainCategory.ListOfCommunicationSumByProduct = listOfCommunicationCount;
        }

        private List<BaseModel> GetCommunicationColumns(
            IEnumerable<LogStatisticsEntity> repositoryData)
        {
            var communicatonToolGroup = repositoryData.GroupBy(data => data.CommunicationId).ToList();

            return communicatonToolGroup.Select(item => new BaseModel
            {
                Id = item.Key,
                Name = item.First().Communication
            }).ToList();
        }

        private List<BaseModel> GetProductColumns(
            IEnumerable<LogStatisticsEntity> repositoryData)
        {
            var productGroup = repositoryData.GroupBy(data => data.ProductId).ToList();

            return productGroup.Select(item => new BaseModel
            {
                Id = item.Key,
                Name = item.First().Product
            }).ToList();
        }

        #endregion GetReportData private methods

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public StatisticsDetailReportByDateModel GetReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            var reportTitle = string.Format(
                Labels.DailyStatisticsReport_DetailTitle,
                ReportHelper.GetLogStatisticSiteTypeName(criteria.SiteType),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());

            criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
            criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
            List<StatisticsDetailbyDateModel> reportDetailData = _logStatisticsRepository.GetReportDetail(criteria).ToList();

            var result = new StatisticsDetailReportByDateModel
            {
                Name = reportTitle,
                ChildReports = reportDetailData,
                TotalChildReports = criteria.TotalRows
            };

            return result;
        }
    }
}