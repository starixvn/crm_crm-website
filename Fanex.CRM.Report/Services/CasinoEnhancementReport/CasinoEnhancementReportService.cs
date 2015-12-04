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
                    Id = criteria.SiteType,
                    Name = reportTitle,
                    AliasName = ChartHelper.ViewChartFromToDateTitle(
                       DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.FromDate).ToShortFormatedDate(),
                       DateTimeUtilize.ChangeTimeZoneToClientDateTime(criteria.ToDate).ToShortFormatedDate()),
                    MainCategories = GetMainCategories(repositoryResult),
                    Products = GetProducts(repositoryResult),
                    CommunicationColumns = GetCommunicationColumns(repositoryResult),
                    ProductColumns = GetProductColumns(repositoryResult)
                };

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

        private IEnumerable<EnhancementReportBaseModel> GetMainCategories(
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData)
        {
            var mainCategories = new List<EnhancementReportBaseModel>();
            var dataGroupByMainCategory = repositoryData.GroupBy(data =>
                     new
                     {
                         MainCategoryId = data.MainCategoryId,
                         MainCategory = data.MainCategory
                     });

            foreach (var reportData in dataGroupByMainCategory)
            {
                int mainCategoryId = reportData.Key.MainCategoryId;
                EnhancementReportBaseModel mainCategory = new EnhancementReportBaseModel
                    {
                        Id = mainCategoryId,
                        Name = reportData.Key.MainCategory,
                        NumberOfCommunication = GetCommunicationCountOfMainCategory(reportData),
                        ChildReports = GetSubCategories(reportData)
                    };

                mainCategories.Add(mainCategory);
            }

            return mainCategories;
        }

        private IEnumerable<int> GetCommunicationCountOfMainCategory(
            IGrouping<dynamic, StatisticByDateEnhancementEntity> mainCategoryGroup)
        {
            var listOfCommunicationCount = new List<int>();
            var dataGroupByCommunication = mainCategoryGroup.GroupBy(data =>
                new
                {
                    SubCategoryId = data.SubCategoryId,
                    CommunicationId = data.CommunicationId,
                });

            foreach (var data in dataGroupByCommunication)
            {
                var sumOfCommunication = data.Sum(p => p.Total);
                listOfCommunicationCount.Add(sumOfCommunication);
            }

            return listOfCommunicationCount;
        }

        private IEnumerable<EnhancementReportBaseModel> GetSubCategories(
            IGrouping<dynamic, StatisticByDateEnhancementEntity> mainCategoryGroup)
        {
            var dataGroupBySubCategory = mainCategoryGroup.GroupBy(s => new { s.SubCategoryId, s.SubCategory });
            var subCategories = new List<EnhancementReportBaseModel>();

            foreach (var data in dataGroupBySubCategory)
            {
                var subCategory = new EnhancementReportBaseModel
                {
                    Id = data.Key.SubCategoryId,
                    Name = data.Key.SubCategory
                };

                subCategories.Add(subCategory);
            }

            return subCategories;
        }

        private IEnumerable<EnhancementReportProduct> GetProducts(
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData)
        {
            var dataGroupByProduct = repositoryData.GroupBy(s => 
                new 
                {
                    s.MainCategoryId,
                    s.MainCategory,
                    s.SubCategoryId,
                    s.SubCategory,
                    s.ProductId, 
                    s.Product 
                });

            var products = new List<EnhancementReportProduct>();

            foreach (var data in dataGroupByProduct)
            {
                var product = new EnhancementReportProduct
                {
                    Id = data.Key.ProductId,
                    Name = data.Key.Product,
                    SubCategoryId = data.Key.SubCategoryId,
                    ChildReports = GetDataDetail(data),
                };

                var numberofCommunication = new List<int>();
                foreach (var communication in data)
                {
                    numberofCommunication.Add(communication.Total);
                }

                product.NumberOfCommunication = numberofCommunication;

                products.Add(product);
            }

            return products;
        }

        private IEnumerable<EnhancementReportBaseModel> GetDataDetail(
            IGrouping<dynamic, StatisticByDateEnhancementEntity> productGroup)
        {
            var communications = new List<EnhancementReportBaseModel>();
            foreach (var data in productGroup)
            {
                var dataDetail = new EnhancementReportBaseModel
                {
                    Id = data.CommunicationId,
                    Name = data.Communication
                };

                communications.Add(dataDetail);
            }

            return communications;
        }

        private List<BaseModel> GetCommunicationColumns(
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData)
        {
            var result = new List<BaseModel>();
            var communicatonToolGroup = repositoryData.GroupBy(data =>
                    new
                    {
                        Id = data.CommunicationId,
                        Name = data.Communication
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

        private List<BaseModel> GetProductColumns(
            IEnumerable<StatisticByDateEnhancementEntity> repositoryData)
        {
            var result = new List<BaseModel>();
            var productGroup = repositoryData.GroupBy(data =>
                    new
                    {
                        Id = data.ProductId,
                        Name = data.Product
                    }).ToList();

            foreach (var item in productGroup)
            {
                var product = new BaseModel
                {
                    Id = item.Key.Id,
                    Name = item.Key.Name
                };
                result.Add(product);
            }

            return result;
        }
    }
}