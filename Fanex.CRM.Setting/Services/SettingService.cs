// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories;
    using Fanex.CRM.Setting.Resources;

    /// <summary>
    /// Setting Service.
    /// </summary>
    public class SettingService : ServiceBase, ISettingService
    {
        public const string GetAllSettingSessionKey = "Fanex.CRM.Setting.GetAllSettingSessionKey";
        public const string GetAllCountrySessionKey = "Fanex.CRM.Setting.GetAllCountrySessionKey";
        public const string GetAllTitleSessionKey = "Fanex.CRM.Setting.GetAllTitleSessionKey";
        public const string GetAllFeedbackTemplateSubjectSessionKey = "Fanex.CRM.Setting.GetAllFeedbackTemplateSubjectSessionKey";

        public const string GetInformHeadlineStatusesSessionKey = "Fanex.CRM.Setting.GetInformHeadlineStatusesSessionKey";
        public const string GetAllSiteSessionKey = "Fanex.CRM.Setting.GetAllSiteSessionKey";
        public const string GetAllSiteWithGroupSessionKey = "Fanex.CRM.Setting.GetAllSiteWithGroupSessionKey";
        public const string GetAllLanguageSessionKey = "Fanex.CRM.Setting.GetAllLanguageSessionKey";
        public const string GetAllExtensionSessionKey = "Fanex.CRM.Setting.GetAllExtensionSessionKey";
        public const string GetAllCommunicationToolSessionKey = "Fanex.CRM.Setting.GetAllCommunicationToolSessionKey";
        public const string GetAllMainCategorySessionKey = "Fanex.CRM.Setting.GetAllMainCategorySessionKey";
        public const string GetAllProductSessionKey = "Fanex.CRM.Setting.GetAllProductSessionKey";
        public const string GetAllStatusSessionKey = "Fanex.CRM.Setting.GetAllStatusSessionKey";
        public const string GetAllStatusCommunicationTool = "Fanex.CRM.Setting.GetAllStatusCommunicationToolSessionKey";
        public const string GetAllStatusRedemption = "Fanex.CRM.Setting.GetAllStatusRedemptionSessionKey";
        public const string GetAllRoleSessionKey = "Fanex.CRM.Setting.GetAllRoleSessionKey";
        public const string GetAllResponseSessionKey = "Fanex.CRM.Setting.GetAllResponseSessionKey";
        public const string GetAllActivityLogActionKey = "Fanex.CRM.Setting.GetAllActivityLogActionKey";
        public const string GetAllSettingLogActionKey = "Fanex.CRM.Setting.GetAllSettingLogActionKey";
        public const string GetAllInformHeadlineSettingKey = "Fanex.CRM.Setting.GetAllInformHeadlineSettingKey";
        public const string GetAllLicenseeTypeSessionKey = "Fanex.CRM.Setting.GetAllLicenseeTypeSessionKey";
        public const string GetAllCurrencySessionKey = "Fanex.CRM.Setting.GetAllCurrencySessionKey";
        public const string GetAllCurrencyRedemptionSessionKey = "Fanex.CRM.Setting.GetAllCurrencyRedemptionSessionKey";

        private readonly ISettingRepository _settingRepositoryInstance;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository settingRepositoryInstance)
        {
            _settingRepositoryInstance = settingRepositoryInstance ?? new SettingRepository();
        }

        /// <summary>
        /// Gets the name of the period type.
        /// </summary>
        /// <param name="periodType">Type of the period.</param>
        /// <returns>The name of the period type.</returns>
        public static string GetPeriodTypeName(int periodType)
        {
            switch (periodType)
            {
                case PeriodTypeConstants.Daily:
                    return Labels.Daily;
                case PeriodTypeConstants.Weekly:
                    return Labels.Weekly;
                case PeriodTypeConstants.Monthly:
                    return Labels.Monthly;
                case PeriodTypeConstants.Yearly:
                    return Labels.Yearly;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the setting by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting by identifier.</returns>
        public SettingInfo GetSettingById(GetSettingByIdCriteria criteria)
        {
            var allSettings = GetAllSettings();
            var setting = allSettings.Where(s => s.Id == criteria.SettingId).FirstOrDefault();
            if (setting == null)
            {
                setting = new SettingInfo()
                {
                    Value = Labels.Symbol_NotApply
                };
            }

            return setting;
        }

        public IEnumerable<SettingInfo> GetAllSettings()
        {
            var allSettings = CacheService.Get<IEnumerable<SettingInfo>>(GetAllSettingSessionKey);
            if (allSettings == null)
            {
                allSettings = _settingRepositoryInstance.GetAllSettings(new GetAllSettingCriteria());
                CacheService.Set(GetAllSettingSessionKey, allSettings);
            }

            return allSettings;
        }

        public IEnumerable<SiteInfo> GetAllSites(GetAllSiteCriteria criteria = null)
        {
            var cache = CacheService.Get<IEnumerable<SiteInfo>>(GetAllSiteSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var allSite = new List<SiteInfo>();
            var crmSite = GetDynamicData<SiteInfo>(criteria ?? new GetAllSiteCriteria());
            var rawDataGroups = crmSite.GroupBy(setting => setting.IsLicensee);
            foreach (var rawDataGroup in rawDataGroups)
            {
                var siteGroup = new SiteInfo
                {
                    Id = rawDataGroup.Key ? SiteConstants.LicenseeGroup : SiteConstants.IBCGroup,
                    Name = rawDataGroup.Key ? Labels.Licensee : Labels.Credit,
                    ChildSites = new List<SiteInfo>()
                };

                foreach (var rawDataSetting in rawDataGroup)
                {
                    var newSite = new SiteInfo()
                    {
                        Name = rawDataSetting.Name,
                        Id = rawDataSetting.Id
                    };

                    siteGroup.ChildSites.Add(newSite);
                }

                allSite.Add(siteGroup);
            }

            CacheService.Set(GetAllSiteSessionKey, allSite);
            return allSite;
        }

        public IEnumerable<BaseModel> GetAllSiteWithGroup()
        {
            var cache = CacheService.Get<IEnumerable<BaseModel>>(GetAllSiteWithGroupSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var sites = new List<BaseModel>();
            var rawdata = GetDynamicData<SiteInfo>(new GetAllSiteCriteria());
            foreach (var item in rawdata)
            {
                var group = item.IsLicensee ? Labels.Licensee : Labels.Credit;
                sites.Add(new BaseModel { Id = item.Id, Name = item.Name, AliasName = group });
            }

            CacheService.Set(GetAllSiteWithGroupSessionKey, sites);
            return sites;
        }

        public IEnumerable<SettingInfo> GetAllTitles()
        {
            var cache = CacheService.Get<IEnumerable<SettingInfo>>(GetAllTitleSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var allTitles = GetSettingRelationByName(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Title,
                ParentId = 0
            });

            CacheService.Set(GetAllTitleSessionKey, allTitles);
            return allTitles;
        }

        public IEnumerable<CountryInfo> GetAllCountries()
        {
            var allCountry = CacheService.Get<IEnumerable<CountryInfo>>(GetAllCountrySessionKey);
            if (allCountry != null)
            {
                return allCountry;
            }

            allCountry = _settingRepositoryInstance.GetAllCountries(new GetAllCountryCriteria());
            CacheService.Set(GetAllCountrySessionKey, allCountry);
            return allCountry;
        }

        public IEnumerable<SettingInfo> GetAllLanguages()
        {
            var allLanguages = CacheService.Get<IEnumerable<SettingInfo>>(GetAllLanguageSessionKey);
            if (allLanguages != null)
            {
                return allLanguages;
            }

            allLanguages = _settingRepositoryInstance.GetAllLanguages(new GetAllLanguageCriteria());
            CacheService.Set(GetAllLanguageSessionKey, allLanguages);
            return allLanguages;
        }

        public IEnumerable<SettingInfo> GetAllExtensions()
        {
            var allExtensions = CacheService.Get<IEnumerable<SettingInfo>>(GetAllExtensionSessionKey);
            if (allExtensions != null)
            {
                return allExtensions;
            }

            allExtensions = GetSettingRelationByName(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Extension,
                ParentId = 0
            });

            CacheService.Set(GetAllExtensionSessionKey, allExtensions);
            return allExtensions;
        }

        public IEnumerable<SettingInfo> GetAllCommunicationTools()
        {
            var tools = CacheService.Get<IEnumerable<SettingInfo>>(GetAllCommunicationToolSessionKey);
            if (tools != null)
            {
                return tools;
            }

            tools = _settingRepositoryInstance.GetAllCommunicationTools(new GetAllCommunicationToolCriteria());
            tools = tools.OrderBy(s => s.DisplayOrder).ToList();
            CacheService.Set(GetAllCommunicationToolSessionKey, tools);

            return tools;
        }

        public IEnumerable<SettingInfo> GetAllMainCategories()
        {
            var cache = CacheService.Get<IEnumerable<SettingInfo>>(GetAllMainCategorySessionKey);
            if (cache != null)
            {
                return cache;
            }
            
            var mainCategories = _settingRepositoryInstance.GetAllMainCategory(new GetAllMainCategoryCriteria());
            var subCategories = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria());

            foreach (var category in mainCategories)
            {
                category.ChildSettings = subCategories.Where(x => x.ParentId == category.Id).OrderBy(s => s.DisplayOrder).ToList();
            }

            CacheService.Set(GetAllMainCategorySessionKey, mainCategories);
            return mainCategories;
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<SettingInfo> GetAllProducts()
        {
            var cache = CacheService.Get<IEnumerable<SettingInfo>>(GetAllProductSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var allProducts = new List<SettingInfo>();
            var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Product,
                ParentId = 0
            });

            var notApplySetting = new SettingInfo
            {
                Id = SettingConstants.NotApply,
                Name = Labels.Symbol_NotApply
            };

            allProducts.Add(new SettingInfo
            {
                Id = SettingConstants.NotApply,
                Value = Labels.Symbol_NotApply,
                ChildSettings = new List<SettingInfo> { notApplySetting }
            });

            foreach (var data in rawData)
            {
                data.ChildSettings = _settingRepositoryInstance.GetAllBetType(new GetAllBetTypeCriteria
                {
                    ParentId = data.Id
                }).ToList();

                data.ChildSettings = data.ChildSettings.OrderBy(s => s.DisplayOrder).ToList();
                data.ChildSettings.Insert(0, notApplySetting);
                allProducts.Add(data);
            }

            CacheService.Set(GetAllProductSessionKey, allProducts);
            return allProducts;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns>
        /// All status.
        /// </returns>
        public IEnumerable<SettingInfo> GetAllStatuses(int statusType)
        {
            string key = GetAllStatusSessionKey;
            if (statusType == ViewTypeConstants.CommunicationTool)
            {
                key = GetAllStatusCommunicationTool;
            }
            else if (statusType == ViewTypeConstants.Redemption)
            {
                key = GetAllStatusRedemption;
            }

            var allStatus = CacheService.Get<IEnumerable<SettingInfo>>(key);
            if (allStatus != null)
            {
                return allStatus;
            }

            allStatus = _settingRepositoryInstance.GetAllStatus(new GetAllStatusCriteria(statusType))
                .OrderBy(s => s.DisplayOrder)
                .ToList();
            CacheService.Set(key, allStatus);

            return allStatus;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns>
        /// All status.
        /// </returns>
        public IEnumerable<Status> GetInformHeadlineStatuses()
        {
            string key = GetInformHeadlineStatusesSessionKey;

            var allStatus = CacheService.Get<IEnumerable<Status>>(key);
            if (allStatus != null)
            {
                return allStatus;
            }

            allStatus = _settingRepositoryInstance.GetStatuses(new GetAllStatusCriteria(ViewTypeConstants.InformHeadline))
                .OrderBy(s => s.DisplayOrder)
                .ToList();
            CacheService.Set(key, allStatus);

            return allStatus;
        }

        /// <summary>
        /// All the currency redemption.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Setting Info List.</returns>
        public IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria)
        {
            string key = GetAllCurrencyRedemptionSessionKey;
            var currencies = CacheService.Get<IEnumerable<BaseModel>>(key);
            if (currencies == null)
            {
                currencies = _settingRepositoryInstance.GetAllCurrencyRedemptions(criteria);
                CacheService.Set(key, currencies);
            }

            return currencies;
        }

        public IEnumerable<SettingInfo> GetAllRoles()
        {
            var allRole = CacheService.Get<IEnumerable<SettingInfo>>(GetAllRoleSessionKey);
            if (allRole != null)
            {
                return allRole;
            }

            allRole = GetSettingRelationByName(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Role,
                ParentId = 0
            });

            CacheService.Set(GetAllRoleSessionKey, allRole);
            return allRole;
        }

        public IEnumerable<SettingInfo> GetAllResponses()
        {
            var allResponse = CacheService.Get<IEnumerable<SettingInfo>>(GetAllResponseSessionKey);
            if (allResponse != null)
            {
                return allResponse;
            }

            allResponse = GetSettingRelationByName(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Response,
                ParentId = 0
            });

            CacheService.Set(GetAllResponseSessionKey, allResponse);
            return allResponse;
        }

        public IEnumerable<SettingInfo> GetAllCustomerTypes()
        {
            var allCustomerType = new List<SettingInfo>();
            allCustomerType.Add(new SettingInfo
            {
                Id = CustomerTypeConstants.All,
                Value = Labels.All
            });

            allCustomerType.Add(new SettingInfo
            {
                Id = CustomerTypeConstants.Customer,
                Value = Labels.Customer
            });

            allCustomerType.Add(new SettingInfo
            {
                Id = CustomerTypeConstants.Guest,
                Value = Labels.Guest
            });

            return allCustomerType;
        }

        public IEnumerable<SettingInfo> GetAllVIPTypes()
        {
            var allVIPType = new List<SettingInfo>();
            allVIPType.Add(new SettingInfo
            {
                Id = VIPTypeConstants.All,
                Value = Labels.All
            });

            allVIPType.Add(new SettingInfo
            {
                Id = VIPTypeConstants.Yes,
                Value = Labels.Yes
            });

            allVIPType.Add(new SettingInfo
            {
                Id = VIPTypeConstants.No,
                Value = Labels.No
            });

            return allVIPType;
        }

        public IEnumerable<SettingInfo> AllHeadlineType()
        {
            return new List<SettingInfo>() 
            { 
                new SettingInfo
                {
                    Id = HeadlineTypeConstants.All,
                    Value = Labels.All
                },
                new SettingInfo
                {
                    Id = HeadlineTypeConstants.Yes,
                    Value = Labels.Yes
                },
                new SettingInfo
                {
                    Id = HeadlineTypeConstants.No,
                    Value = Labels.No
                }
            };
        }

        public IEnumerable<BaseModel> GetAllLicenseeTypes()
        {
            string key = GetAllLicenseeTypeSessionKey;
            var cache = CacheService.Get<IEnumerable<BaseModel>>(key);
            if (cache != null)
            {
                return cache;
            }

            var licenseeTypes = _settingRepositoryInstance.GetAllLicenseeTypes(new GetAllLicenseeTypeCriteria()).ToList();
            licenseeTypes.Insert(
                0,
                new BaseModel
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.Symbol_NotApply
                });

            CacheService.Set(key, licenseeTypes);
            return licenseeTypes;
        }

        /// <summary>
        /// Gets all currency.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="hasDefaultOption">Has N/A option or not.</param>
        /// <returns>The currency list.</returns>
        public IEnumerable<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria, bool hasDefaultOption = true)
        {
            string key = GetAllCurrencySessionKey;
            var cache = CacheService.Get<IEnumerable<BaseModel>>(key);
            if (cache != null)
            {
                return cache;
            }

            var currencies = _settingRepositoryInstance.GetAllCurrencies(criteria).ToList();
            CacheService.Set(key, currencies);

            if (hasDefaultOption)
            {
                currencies.Insert(
                    0,
                    new BaseModel
                    {
                        Id = SettingConstants.NotApply,
                        Name = Labels.Symbol_NotApply
                    });
            }

            return currencies;
        }

        /// <summary>
        /// Gets the sub setting by parent identifier.
        /// </summary>
        /// <param name="parentSetting">The parent setting.</param>
        /// <param name="allSettingRelation">All setting relation.</param>
        /// <param name="allSetting">All setting.</param>
        /// <returns>The sub setting by parent identifier.</returns>
        public IEnumerable<SettingInfo> GetSubSettingByParentId(
            BaseModel parentSetting,
            IEnumerable<SettingInfo> allSettingRelation,
            IEnumerable<SettingInfo> allSetting)
        {
            var subSettings = new List<SettingInfo>();
            if (allSettingRelation == null || !allSettingRelation.Any())
            {
                return subSettings;
            }

            foreach (var settingRelation in allSettingRelation)
            {
                if (settingRelation.ParentId == parentSetting.Id)
                {
                    for (var i = 0; i < allSetting.Count(); i++)
                    {
                        var setting = allSetting.ElementAt(i);
                        if (setting.Id == settingRelation.Id)
                        {
                            subSettings.Add(new SettingInfo
                            {
                                Id = setting.Id,
                                DisplayOrder = setting.DisplayOrder,
                                Value = setting.Value,
                                SettingType = setting.SettingType,
                                ParentId = parentSetting.Id,
                                IsSubSetting = true
                            });
                        }
                    }
                }
            }

            subSettings = subSettings.OrderBy(s => s.DisplayOrder).ToList();
            return subSettings;
        }

        public int NumberOfFeedbackSubjectTemplate()
        {
            var feedbackTemplates = GetFeedbackSubjectTemplates();
            return feedbackTemplates.Count();
        }

        public IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var result = new List<FeedbackTemplate>();
            int multipleNum = SettingConstants.MaxFeedbackSubjectTemplateNumberOnLoad;
            var feedbackTemplates = CacheService.Get<IEnumerable<FeedbackTemplate>>(GetAllFeedbackTemplateSubjectSessionKey);

            if (feedbackTemplates == null)
            {
                feedbackTemplates = GetFeedbackSubjectTemplates();
            }

            if (feedbackTemplates != null && feedbackTemplates.Count() > 0)
            {
                result = feedbackTemplates
                    .Skip((criteria.PagingIndex - 1) * multipleNum)
                    .Take(criteria.PagingIndex * multipleNum).ToList();
            }

            return result;
        }

        public IEnumerable<BaseModel> GetAllLogActions(GetActionCriteria criteria)
        {
            string cachedKey = criteria.ActionType == SettingConstants.ActivityLog
                ? GetAllActivityLogActionKey
                : GetAllSettingLogActionKey;
            var actions = CacheService.Get<IEnumerable<BaseModel>>(cachedKey);
            if (actions != null)
            {
                return actions;
            }

            actions = _settingRepositoryInstance.GetAllLogActions(criteria);
            CacheService.Set(cachedKey, actions);
            return actions;
        }

        public IEnumerable<SettingInfo> GetAllSiteTypes()
        {
            return new List<SettingInfo>()
            {
                new SettingInfo { Id = SiteConstants.All, Value = Labels.All },
                new SettingInfo { Id = SiteConstants.IBCGroup, Value = Labels.Credit },
                new SettingInfo { Id = SiteConstants.LicenseeGroup, Value = Labels.Licensee },
            };
        }

        public InformHeadlineSetting GetAllInformHeadlineSettings()
        {
            try
            {
                string cachedKey = GetAllInformHeadlineSettingKey;
                var settings = CacheService.Get<InformHeadlineSetting>(cachedKey);
                if (settings != null)
                {
                    return settings;
                }

                var roles = new List<BaseModel> 
                {
                    new BaseModel { Id = RoleIdConstants.Member, Name = Labels.Member },
                    new BaseModel { Id = RoleIdConstants.Agent, Name = Labels.Agent },
                    new BaseModel { Id = RoleIdConstants.Master, Name = Labels.Master },
                    new BaseModel { Id = RoleIdConstants.Super, Name = Labels.Super }
                };

                settings = new InformHeadlineSetting
                {
                    Status = _settingRepositoryInstance.GetAllStatusInformHeadline(new GetAllStatusInformHeadlineCriteria()),
                    Position = roles,
                    Currency = GetAllCurrencies(new GetAllCurrencyCriteria()),
                    Site = GetAllSites().ElementAt(0).ChildSites.Select(s => new BaseModel { Id = s.Id, Name = s.Name }).ToList(),
                    ProductStatus = new List<BaseModel>() 
                    {
                        new BaseModel { Id = HeadlineProductStatusConstants.NotAvailable, Name = Labels.Symbol_NotApply },
                        new BaseModel { Id = HeadlineProductStatusConstants.Enable, Name = Labels.Enable },
                        new BaseModel { Id = HeadlineProductStatusConstants.Disable, Name = Labels.Disable }
                    },
                    Extensions = GetAllExtensions()
                };

                CacheService.Set(cachedKey, settings);
                return settings;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public BaseModel[] GetFeedbackStatisticFilterTypes()
        {
            return new BaseModel[]
            {
                new BaseModel { Id = FeedbackStatisticFilterType.ByCreator, Name = Labels.ByCreator },
                new BaseModel { Id = FeedbackStatisticFilterType.ByCountry, Name = Labels.ByCountry },
                new BaseModel { Id = FeedbackStatisticFilterType.ByRole, Name = Labels.ByRole },
                new BaseModel { Id = FeedbackStatisticFilterType.BySite, Name = Labels.BySite },
                new BaseModel { Id = FeedbackStatisticFilterType.ByStatus, Name = Labels.ByStatus },
                new BaseModel { Id = FeedbackStatisticFilterType.ByResponse, Name = Labels.ByResponse },
                new BaseModel { Id = FeedbackStatisticFilterType.ByProduct, Name = Labels.ByProduct },
                new BaseModel { Id = FeedbackStatisticFilterType.ByCategory, Name = Labels.ByCategory },
                new BaseModel { Id = FeedbackStatisticFilterType.ByLicensees, Name = Labels.ByLicensees },
                new BaseModel { Id = FeedbackStatisticFilterType.ByCommunicationTool, Name = Labels.ByCommunicationTool },
            };
        }

        public void ClearCache()
        {
            CacheService.Clear();
        }

        public IEnumerable<SettingInfo> GetMainSettings()
        {
            var allSettings = GetAllSettings();
            var settingGroups = allSettings
                .GroupBy(x => new { Type = x.SettingType })
                .Select(group => new { group.Key, Count = group.Count() });
            var settingGroupByType = new List<SettingInfo>();
            foreach (var s in settingGroups)
            {
                GetSubSetting(s.Key.Type, settingGroupByType, allSettings);
            }

            return settingGroupByType.OrderBy(s => s.DisplayOrder).ToList();
        }

        public IEnumerable<Station> GetStations()
        {
            return _settingRepositoryInstance.GetStations(new GetStationsCriteria());
        }

        private void GetSubSetting(string key, IList<SettingInfo> settingList, IEnumerable<SettingInfo> allSettings)
        {
            var subList = allSettings.Where(s => s.SettingType == key).Select(s => s).OrderBy(s => s.DisplayOrder).ToList();
            var setting = new SettingInfo { SettingType = key, ChildSettings = subList };
            settingList.Add(setting);
        }

        private IEnumerable<SettingInfo> GetSettingRelationByName(GetSettingRelationByNameCriteria criteria)
        {
            var settingRelations = _settingRepositoryInstance.GetSettingRelations(criteria);

            return settingRelations.OrderBy(setting => setting.DisplayOrder).ToList();
        }

        private IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplates()
        {
            var feedbackTemplates = _settingRepositoryInstance.GetFeedbackSubjectTemplateById(new GetFeedbackSubjectTemplateByIdCriteria
            {
                TemplateId = 0
            });

            CacheService.Set(GetAllFeedbackTemplateSubjectSessionKey, feedbackTemplates);
            return feedbackTemplates;
        }
    }
}
