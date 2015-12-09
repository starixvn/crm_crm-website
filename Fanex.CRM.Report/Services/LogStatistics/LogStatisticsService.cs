// -----------------------------------------------------------------------
// <copyright file="LogStatisticService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.LogStatistics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Helpers;
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
            try
            {
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

                var repositoryResult = _logStatisticsRepository.GetReportData(criteria);

                var reportTitle = string.Format(
                    Labels.LogStatisticsReport_Title,
                    ReportHelper.GetLogStatisticSiteTypeName(criteria.SiteType),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());

                var logStatistic = new LogStatisticsViewModel
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

        #region GetReportData private methods

        private IEnumerable<LogStatisticsMainCategory> GetMainCategories(
            IEnumerable<LogStatisticsEntity> repositoryData, int productLength, int communicationLength)
        {
            var dataGroupByMainCategory = repositoryData.GroupBy(data =>
                     new
                     {
                         MainCategoryId = data.MainCategoryId,
                         MainCategory = data.MainCategory
                     });

            return dataGroupByMainCategory.Select(reportData => new LogStatisticsMainCategory
            {
                Id = reportData.Key.MainCategoryId,
                Name = reportData.Key.MainCategory,
                SubCategories = GetSubCategories(reportData),
                ListOfCommunicationSumByProduct = Enumerable.Repeat(0, productLength).ToList(),
                ListOfCommunicationCount = Enumerable.Repeat(0, productLength * communicationLength).ToList()
            }).ToList();
        }

        private IEnumerable<LogStatisticsBaseModel> GetSubCategories(
            IGrouping<dynamic, LogStatisticsEntity> mainCategoryGroup)
        {
            var dataGroupBySubCategory = mainCategoryGroup.GroupBy(s => new { s.SubCategoryId, s.SubCategory });

            return dataGroupBySubCategory.Select(data => new LogStatisticsBaseModel
            {
                Id = data.Key.SubCategoryId,
                Name = data.Key.SubCategory,
                ListOfCommunicationSumByProduct = GetSumOfCommunicationByProductForSubCategory(data),
                ListOfCommunicationCount = GetNumberOfCommunicationForSubCategory(data)
            }).ToList();
        }

        private List<int> GetSumOfCommunicationByProductForSubCategory(
            IGrouping<dynamic, LogStatisticsEntity> subCategoryGroup)
        {
            var communicatonToolGroup = subCategoryGroup.GroupBy(data =>
                    new
                    {
                        ProductId = data.ProductId,
                    }).ToList();

            return communicatonToolGroup.ToList().Select(data => data.Sum(p => p.Total)).ToList();
        }

        private List<int> GetNumberOfCommunicationForSubCategory(
            IGrouping<dynamic, LogStatisticsEntity> subCategoryGroup)
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
            LogStatisticsBaseModel mainCategory, List<int> numBerOfCommunicationBySubCategory)
        {
            for (int i = 0; i < numBerOfCommunicationBySubCategory.Count; i++)
            {
                mainCategory.ListOfCommunicationCount[i] += numBerOfCommunicationBySubCategory[i];
            }
        }

        private void SumCommunicationCountByProductForMainCategory(
            LogStatisticsBaseModel mainCategory, List<int> sumOfCommunicationBySubCategory)
        {
            for (int i = 0; i < sumOfCommunicationBySubCategory.Count; i++)
            {
                mainCategory.ListOfCommunicationSumByProduct[i] += sumOfCommunicationBySubCategory[i];
            }
        }

        private List<BaseModel> GetCommunicationColumns(
            IEnumerable<LogStatisticsEntity> repositoryData)
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
            IEnumerable<LogStatisticsEntity> repositoryData)
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

        #endregion

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public StatisticsDetailReportByDateModel GetReportDetail(LogStatisticsReportDetailCriteria criteria)
        {
            try
            {
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);
                List<StatisticsDetailbyDateModel> reportDetailData = _logStatisticsRepository.GetReportDetail(criteria).ToList();

                var reportTitle = string.Format(
                    Labels.DailyStatisticsReport_DetailTitle,
                    ReportHelper.GetLogStatisticSiteTypeName(criteria.SiteType),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());
                var result = new StatisticsDetailReportByDateModel
                {
                    Name = reportTitle,
                    ChildReports = new List<StatisticsDetailbyDateModel>(),
                    StartChildIndex = criteria.StartIndex,
                    TotalChildReports = reportDetailData.Count
                };

                List<StatisticsDetailbyDateModel> pagingData = criteria.IsGetFullReport ?
                    reportDetailData :
                    reportDetailData.Skip(criteria.StartIndex).Take(criteria.PageSize).ToList();
                foreach (var data in pagingData)
                {
                    var feedbackDate = (DateTime)DateTimeUtilize.ChangeTimeZoneToClientDateTime(data.StartDate);
                    var resolutionDate = (DateTime)DateTimeUtilize.ChangeTimeZoneToClientDateTime(data.ResolutionTime);
                    var timeSpan = resolutionDate - feedbackDate;

                    var detailInfo = new StatisticsDetailbyDateModel
                    {
                        Remark = data.Remark,
                        Solution = data.Solution,
                        StartDateString = feedbackDate.ToShortFormatedDate(),
                        StartTime = feedbackDate.ToHourMinuteMiddayTypeFormated(),
                        Duration = new TimeInfo
                        {
                            Days = timeSpan.Days,
                            Hours = timeSpan.Hours,
                            Minutes = timeSpan.Minutes
                        },
                        Username = data.Username,
                        SiteInfo = data.SiteInfo,
                        Product = data.Product,
                        Extension = data.Extension,
                        Country = data.Country,
                        MainCategory = data.MainCategory,
                        SubCategory = data.SubCategory,
                        StatusName = data.StatusName,
                        Response = data.Response,
                        Handler = data.Handler,
                        CreationTime = data.CreationTime,
                        CreationTimeString = DateTimeUtilize.ChangeTimeZoneToClient(data.CreationTime),
                        ResolutionTimeString = resolutionDate.ToHourMinuteMiddayTypeFormated(),

                        Role = data.Role,
                        CommunicationTool = data.CommunicationTool,
                        Phones = data.Phones,
                        PICs = data.PICs,
                        Fax = data.Fax,
                        MobilePhone = data.MobilePhone,
                        MSN = data.MSN,
                        Skype = data.Skype,
                        Email = data.Email,
                        Bbm = data.Bbm,
                        WhatsApp = data.WhatsApp,
                        Line = data.Line,
                        WEChat = data.WEChat,
                        Yahoo = data.Yahoo,
                        QQ = data.QQ
                    };

                    result.ChildReports.Add(detailInfo);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}