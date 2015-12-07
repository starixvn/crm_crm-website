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
    using Fanex.CRM.Report.Models.CasinoReportEnhancement;
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

        public CasinoReportEnhancementViewModel GetReportData(CasinoEnhancementReportCriteria criteria)
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

                var casinoReportEnhancement = new CasinoReportEnhancementViewModel
                {
                    Id = 1,
                    Name = reportTitle,
                    CommunicationColumns = GetCommunicationColumns(repositoryResult),
                    ProductColumns = GetProductColumns(repositoryResult)
                };

                int productLength = casinoReportEnhancement.ProductColumns.Count();
                int communicationLength = casinoReportEnhancement.CommunicationColumns.Count();

                casinoReportEnhancement.MainCategories = GetMainCategories(repositoryResult, productLength, communicationLength);
                SumTotalCommunicationForMainCategory(casinoReportEnhancement.MainCategories);

                return casinoReportEnhancement;
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

        private IEnumerable<EnhancementReportMainCategory> GetMainCategories(
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData, int productLength, int communicationLength)
        {
            var dataGroupByMainCategory = repositoryData.GroupBy(data =>
                     new
                     {
                         MainCategoryId = data.MainCategoryId,
                         MainCategory = data.MainCategory
                     });

            return dataGroupByMainCategory.Select(reportData => new EnhancementReportMainCategory
            {
                Id = reportData.Key.MainCategoryId,
                Name = reportData.Key.MainCategory,
                SubCategories = GetSubCategories(reportData),
                ListOfCommunicationSumByProduct = Enumerable.Repeat(0, productLength).ToList(),
                ListOfCommunicationCount = Enumerable.Repeat(0, productLength * communicationLength).ToList()
            }).ToList();
        }

        private IEnumerable<EnhancementReportBaseModel> GetSubCategories(
            IGrouping<dynamic, StatisticByDateEnhancementEntity> mainCategoryGroup)
        {
            var dataGroupBySubCategory = mainCategoryGroup.GroupBy(s => new { s.SubCategoryId, s.SubCategory });

            return dataGroupBySubCategory.Select(data => new EnhancementReportBaseModel
            {
                Id = data.Key.SubCategoryId,
                Name = data.Key.SubCategory,
                ListOfCommunicationSumByProduct = GetSumOfCommunicationByProductForSubCategory(data),
                ListOfCommunicationCount = GetNumberOfCommunicationForSubCategory(data)
            }).ToList();
        }

        private List<int> GetSumOfCommunicationByProductForSubCategory(
            IGrouping<dynamic, StatisticByDateEnhancementEntity> subCategoryGroup)
        {
            var communicatonToolGroup = subCategoryGroup.GroupBy(data =>
                    new
                    {
                        ProductId = data.ProductId,
                    }).ToList();

            return communicatonToolGroup.ToList().Select(data => data.Sum(p => p.Total)).ToList();
        }

        private List<int> GetNumberOfCommunicationForSubCategory(
            IGrouping<dynamic, StatisticByDateEnhancementEntity> subCategoryGroup)
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

        private void SumTotalCommunicationForMainCategory(IEnumerable<EnhancementReportMainCategory> mainCategories)
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
            EnhancementReportBaseModel mainCategory, List<int> numBerOfCommunicationBySubCategory)
        {
            for (int i = 0; i < numBerOfCommunicationBySubCategory.Count; i++)
            {
                mainCategory.ListOfCommunicationCount[i] += numBerOfCommunicationBySubCategory[i];
            }
        }

        private void SumCommunicationCountByProductForMainCategory(
            EnhancementReportBaseModel mainCategory, List<int> sumOfCommunicationBySubCategory)
        {
            for (int i = 0; i < sumOfCommunicationBySubCategory.Count; i++)
            {
                mainCategory.ListOfCommunicationSumByProduct[i] += sumOfCommunicationBySubCategory[i];
            }
        }

        private List<BaseModel> GetCommunicationColumns(
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData)
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
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData)
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