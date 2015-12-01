// -----------------------------------------------------------------------
// <copyright file="CasinoEnhancementReportService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Services.CasinoEnhancementReport
{
    using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.CRM.Common.Helpers;
using Fanex.CRM.Infrastructure.Extensions;
using Fanex.CRM.Infrastructure.Helpers;
using Fanex.CRM.Infrastructure.Models;
using Fanex.CRM.Report.Criteria.CasinoEnhancementReport;
using Fanex.CRM.Report.Entities.StatisticsReportByDate;
using Fanex.CRM.Report.Models;
using Fanex.CRM.Report.Models.StatisticsReportByDate;
using Fanex.CRM.Report.Repositories.CasinoReportEnhancement;

    public class CasinoEnhancementReportService : ICasinoEnhancementReportService
    {
        private readonly ICasinoReportEnhancementRepository _casinoReportEnhancementRepository;

        public CasinoEnhancementReportService()
            : this(null)
        {
        }

        public CasinoEnhancementReportService(ICasinoReportEnhancementRepository casinoReportEnhancementRepository)
        {
            _casinoReportEnhancementRepository = casinoReportEnhancementRepository ?? new CasinoReportEnhancementRepository();
        }

        public StatisticsReportByDateModel GetReportData(CasinoEnhancementReportCriteria criteria)
        {
            try
            {
                criteria.FromDate = DateTimeUtilize.ChangeClientFromTimeToServerTime(criteria.FromDate);
                criteria.ToDate = DateTimeUtilize.ChangeClientToTimeToServerTime(criteria.ToDate);

                var repositoryResult = _casinoReportEnhancementRepository.GetReportData(criteria);

                var reportTitle = string.Format(
                    MainReportTitle(),
                    SiteHelper.SiteTypeName(criteria.SiteType),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                    DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate());

                var result = new StatisticsReportByDateModel
                {
                    Id = criteria.SiteType,
                    Name = reportTitle,
                    AliasName = ChartHelper.ViewChartFromToDateTitle(
                       DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                       DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                    ChildReports = new List<CategoryIssuesInfo>(),
                    NumberOfIssueList = Enumerable.Repeat(0, 1).ToList(),
                    CommunicationTools = GetCommunicationColumns(repositoryResult.RawData),
                };

                var reportDataGroups = repositoryResult.RawData.GroupBy(data =>
                     new
                     {
                         MainCategoryId = data.MainCategoryId,
                         MainCategoryName = data.MainCategoryName
                     });

                foreach (var reportData in reportDataGroups)
                {
                    var categoryIssuesInfo = new CategoryIssuesInfo(reportData.Key.MainCategoryId, reportData.Key.MainCategoryName);
                    categoryIssuesInfo.NumberOfIssueList = Enumerable.Repeat(0, 1).ToList();
                    categoryIssuesInfo.ChildReports = GetSubCategories(reportData);

                    result.ChildReports.Add(categoryIssuesInfo);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public virtual string MainReportTitle()
        {
            return string.Empty;
        }

        private List<CategoryIssuesInfo> GetSubCategories(IGrouping<dynamic, StatisticByDateEnhancementEntity> mainCategoryGroup)
        {
            var subCategoryGroup = mainCategoryGroup.GroupBy(s => new { s.SubCategoryId, s.SubCategoryName });
            var result = new List<CategoryIssuesInfo>();
            foreach (var subCategory in subCategoryGroup)
            {
                var subCategoryInfo = new CategoryIssuesInfo
                {
                    Id = subCategory.Key.SubCategoryId,
                    Name = subCategory.Key.SubCategoryName,
                    ChildReports = GetCommunications(subCategory),
                    NumberOfIssueList = Enumerable.Repeat(0, 1).ToList(),
                };

                result.Add(subCategoryInfo);
            }

            return result;
        }

        private List<CategoryIssuesInfo> GetCommunications(IGrouping<dynamic, StatisticByDateEnhancementEntity> subCategoryGroup)
        {
            var communicationGroup = subCategoryGroup.GroupBy(s => new { s.CommunicationId, s.CommunicationName });
            var result = new List<CategoryIssuesInfo>();
            foreach (var communication in communicationGroup)
            {
                var subCategoryInfo = new CategoryIssuesInfo
                {
                    Id = communication.Key.CommunicationId,
                    Name = communication.Key.CommunicationName,
                    ChildReports = GetDataDetail(communication),
                    NumberOfIssueList = Enumerable.Repeat(0, 1).ToList(),
                };

                result.Add(subCategoryInfo);
            }

            return result;
        }

        private List<CategoryIssuesInfo> GetDataDetail(IGrouping<dynamic, StatisticByDateEnhancementEntity> communicationGroup)
        {
            var result = new List<CategoryIssuesInfo>();
            foreach (var data in communicationGroup)
            {
                var dataDetail = new CategoryIssuesInfo
                {
                    Id = data.ProductId,
                    Name = data.ProductName,
                    NumberOfIssueList = new List<int> { data.Total },
                };

                result.Add(dataDetail);
            }

            return result;
        }

        private List<BaseModel> GetCommunicationColumns(List<StatisticByDateEnhancementEntity> rawData)
        {
            var result = new List<BaseModel>();
            var communicatonToolGroup = rawData.GroupBy(data =>
                    new
                    {
                        Id = data.CommunicationId,
                        Name = data.CommunicationName
                    }).ToList();

            foreach (var item in communicatonToolGroup)
            {
                var communicationTool = new BaseModel
                {
                    Id = item.Key.Id,
                    Name = item.Key.Name
                };
                result.Add(communicationTool);
            }

            return result;
        }
    }
}