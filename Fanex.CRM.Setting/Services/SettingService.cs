// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services
{
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories;
    using Fanex.CRM.Setting.Resources;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

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
        public const string GetAllMainCategoryFromAllSettingsSessionKey = "Fanex.CRM.Setting.GetAllMainCategoryFromAllSettingsSessionKey";
        public const string GetProductOnlySessionKey = "Fanex.CRM.Setting.GetProductOnlySessionKey";
        public const string GetAllProductSessionKey = "Fanex.CRM.Setting.GetAllProductSessionKey";
        public const string GetAllSubProductSessionKey = "Fanex.CRM.Setting.GetAllSubProductSessionKey";
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
        public const string GetProductsSessionKey = "Fanex.CRM.Setting.GetProductsSessionKey";
        public const string GetSubProductsSessionKey = "Fanex.CRM.Setting.GetSubProductsSessionKey";
        public const string GetBettypesSessionKey = "Fanex.CRM.Setting.GetBettypesSessionKey";
        public const string GetMainCategorySessionKey = "Fanex.CRM.Setting.GetMainCategorySessionKey";
        public const string GetSubCategorySessionKey = "Fanex.CRM.Setting.GetSubCategorySessionKey";

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
        public Setting GetSettingById(GetSettingByIdCriteria criteria)
        {
            var setting = GetAllSettings().Where(s => s.Id == criteria.SettingId)
                .FirstOrDefault();
            return setting ?? new Setting
            {
                Value = Labels.Symbol_NotApply

            };
        }

        public IEnumerable<Setting> GetAllSettings()
        {
            var allSettings = CacheService.Get<IEnumerable<Setting>>(GetAllSettingSessionKey);
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

            var allSites = (from n in GetDynamicData<SiteInfo>(criteria ?? new GetAllSiteCriteria())
                            where n.IsActive
                            group n by n.IsLicensee into g
                            orderby g.Key
                            select new SiteInfo
                            {
                                Id = g.Key ? SiteConstants.LicenseeGroup : SiteConstants.IBCGroup,
                                Name = g.Key ? Labels.Licensee : Labels.Credit,
                                ChildSites = (from c in g
                                              orderby c.Name
                                              select new SiteInfo
                                              {
                                                  Id = c.Id,
                                                  Name = c.Name
                                              }).ToList()

                            }).ToList();

            CacheService.Set(GetAllSiteSessionKey, allSites);
            return allSites;
        }

        public IEnumerable<SiteSetting> GetAllSiteWithGroup()
        {
            var cache = CacheService.Get<IEnumerable<SiteSetting>>(GetAllSiteWithGroupSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var sites = new List<SiteSetting>();
            var rawdata = GetDynamicData<SiteInfo>(new GetAllSiteCriteria());
            foreach (var item in rawdata)
            {
                var group = item.IsLicensee ? Labels.Licensee : Labels.Credit;
                sites.Add(new SiteSetting
                {
                    Id = item.Id,
                    Name = item.Name,
                    AliasName = group,
                    IsActive = item.IsActive
                });
            }

            sites = sites.OrderBy(s => s.AliasName).ThenBy(s => s.Name).ToList();
            CacheService.Set(GetAllSiteWithGroupSessionKey, sites);
            return sites;
        }

        public IEnumerable<SettingRelation> GetAllTitles()
        {
            var cache = CacheService.Get<IEnumerable<SettingRelation>>(GetAllTitleSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var allTitles = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Title,
                ParentId = 0
            }).ToList();

            CacheService.Set(GetAllTitleSessionKey, allTitles);

            return allTitles;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            var allCountry = CacheService.Get<IEnumerable<Country>>(GetAllCountrySessionKey);
            if (allCountry != null)
            {
                return allCountry;
            }

            allCountry = _settingRepositoryInstance.GetAllCountries(new GetAllCountryCriteria());
            CacheService.Set(GetAllCountrySessionKey, allCountry);
            return allCountry;
        }

        public IEnumerable<Setting> GetAllLanguages()
        {
            var allLanguages = CacheService.Get<IEnumerable<Setting>>(GetAllLanguageSessionKey);
            if (allLanguages != null)
            {
                return allLanguages;
            }

            allLanguages = _settingRepositoryInstance.GetAllLanguages(new GetAllLanguageCriteria());
            CacheService.Set(GetAllLanguageSessionKey, allLanguages);
            return allLanguages;
        }

        public IEnumerable<SettingRelation> GetAllExtensions()
        {
            var allExtensions = CacheService.Get<IEnumerable<SettingRelation>>(GetAllExtensionSessionKey);
            if (allExtensions != null)
            {
                return allExtensions;
            }

            allExtensions = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Extension,
                ParentId = 0
            })
            .Where(e => e.Value != "N/A");

            CacheService.Set(GetAllExtensionSessionKey, allExtensions);
            return allExtensions;
        }

        public SettingRelation GetExtensionNA()
        {
            var allExtensions = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Extension,
                ParentId = 0
            });

            return allExtensions.FirstOrDefault(e => e.Value == "N/A");
        }

        public IEnumerable<Setting> GetAllCommunicationTools()
        {
            var tools = CacheService.Get<IEnumerable<Setting>>(GetAllCommunicationToolSessionKey);
            if (tools != null)
            {
                return tools;
            }

            tools = _settingRepositoryInstance.GetAllCommunicationTools(new GetAllCommunicationToolCriteria());
            tools = tools.Where(t => t.Name != "N/A").OrderBy(s => s.DisplayOrder).ToList();
            CacheService.Set(GetAllCommunicationToolSessionKey, tools);

            return tools;
        }

        public Product GetProductNA()
        {
            var products = GetSettingRelationByName<Product>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Product,
                ParentId = 0
            });

            return products.FirstOrDefault(p => p.Value == "N/A.");
        }

        public Product GetSubProductNA()
        {
            var subProducts = _settingRepositoryInstance.GetSubProducts(new SubProductCriteria());

            return subProducts.FirstOrDefault(s => s.Value == "N/A.");
        }

        public Product GetBettypeNA()
        {
            var betTypes = _settingRepositoryInstance.GetAllBetType(new GetAllBetTypeCriteria());

            return betTypes.FirstOrDefault(b => b.Name == "N/A.");
        }

        public IEnumerable<Setting> GetAllMainCategories()
        {
            var cache = CacheService.Get<IEnumerable<Setting>>(GetAllMainCategorySessionKey);
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

        public IEnumerable<Setting> GetMainCategories()
        {
            var cache = CacheService.Get<IEnumerable<Setting>>(GetMainCategorySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var mainCategories = _settingRepositoryInstance.GetAllMainCategory(new GetAllMainCategoryCriteria());

            CacheService.Set(GetMainCategorySessionKey, mainCategories);
            return mainCategories;
        }

        public IEnumerable<Setting> GetSubCategories()
        {
            var cache = CacheService.Get<IEnumerable<Setting>>(GetSubCategorySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var subCategories = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria());

            CacheService.Set(GetSubCategorySessionKey, subCategories);
            return subCategories;
        }

        public IEnumerable<Setting> GetAllMainCategoriesFromAllSettings()
        {
            var cache = CacheService.Get<IEnumerable<Setting>>(GetAllMainCategoryFromAllSettingsSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var mainCategories = GetAllSettings().Where(s => s.SettingType.Equals(SettingTypeConstants.MainCategory));
            var subCategories = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria());

            foreach (var category in mainCategories)
            {
                category.ChildSettings = subCategories.Where(x => x.ParentId == category.Id).OrderBy(s => s.DisplayOrder).ToList();
            }

            CacheService.Set(GetAllMainCategoryFromAllSettingsSessionKey, mainCategories);
            return mainCategories;
        }

        public IEnumerable<Product> GetProductsOnly()
        {
            var cache = CacheService.Get<IEnumerable<Product>>(GetProductOnlySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var products = GetSettingRelationByName<Product>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Product,
                ParentId = 0
            }).ToList();

            CacheService.Set(GetProductOnlySessionKey, products);

            return products;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var cache = CacheService.Get<IEnumerable<Product>>(GetAllProductSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var parentProducts = GetProductsOnly();
            var products = new List<Product>();

            var subProducts = GetAllSubProducts();

            foreach (var parentProduct in parentProducts)
            {
                var subProductByProduct = subProducts.Where(p => p.ParentId == parentProduct.Id).ToList();
                parentProduct.ChildSettings = subProductByProduct;
                parentProduct.Name = parentProduct.Value;

                if (parentProduct.Name != "N/A.")
                {
                    products.Add(parentProduct);
                }
            }

            CacheService.Set(GetAllProductSessionKey, products);

            return products;
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<Product> GetAllSubProducts()
        {
            var cache = CacheService.Get<IEnumerable<Product>>(GetAllSubProductSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var products = new List<Product>();
            var subProducts = _settingRepositoryInstance.GetSubProducts(new SubProductCriteria());

            var betTypes = _settingRepositoryInstance.GetAllBetType(new GetAllBetTypeCriteria());
            foreach (var data in subProducts)
            {
                var childSettings = betTypes.Where(s => s.ParentId == data.Id && s.Name != "N/A.")
                    .Select(s => { s.GrandParentId = data.ParentId; return s; })
                    .OrderBy(s => s.DisplayOrder)
                    .ToList();
                data.ChildSettings = childSettings;
                data.Name = data.Value;

                if (data.Name != "N/A.")
                {
                    products.Add(data);
                }
            }

            CacheService.Set(GetAllSubProductSessionKey, products);
            return products;
        }

        public IEnumerable<Product> GetProducts()
        {
            var cache = CacheService.Get<IEnumerable<Product>>(GetProductsSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var products = GetProductsOnly()
                              .Where(p => p.Value != "N/A.")
                              .Select(p => { p.Name = p.Value; return p; })
                              .ToList();

            CacheService.Set(GetProductsSessionKey, products);
            return products;
        }

        public IEnumerable<Product> GetSubProducts()
        {
            var cache = CacheService.Get<IEnumerable<Product>>(GetSubProductsSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var subProducts = _settingRepositoryInstance
                                .GetSubProducts(new SubProductCriteria())
                                .Where(p => p.Value != "N/A." && p.ParentId != 0)
                                .Select(p => { p.Name = p.Value; return p; })
                                .ToList();

            CacheService.Set(GetSubProductsSessionKey, subProducts);
            return subProducts;
        }

        public IEnumerable<Product> GetBettypes()
        {
            var cache = CacheService.Get<IEnumerable<Product>>(GetBettypesSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var betTypes = _settingRepositoryInstance
                               .GetAllBetType(new GetAllBetTypeCriteria())
                               .Where(b => b.Name != "N/A.")
                               .OrderBy(b => b.DisplayOrder)
                               .ToList();

            CacheService.Set(GetBettypesSessionKey, betTypes);
            return betTypes;
        }


        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns>
        /// All status.
        /// </returns>
        public IEnumerable<BaseModel> GetAllStatuses(int statusType)
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

            var allStatus = CacheService.Get<IEnumerable<BaseModel>>(key);
            if (allStatus != null)
            {
                return allStatus;
            }

            allStatus = _settingRepositoryInstance.GetAllStatus(new GetAllStatusCriteria(statusType));
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

        public IEnumerable<SettingRelation> GetAllRoles()
        {
            var allRole = CacheService.Get<IEnumerable<SettingRelation>>(GetAllRoleSessionKey);
            if (allRole != null)
            {
                return allRole;
            }

            allRole = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Role,
                ParentId = 0
            });

            CacheService.Set(GetAllRoleSessionKey, allRole);
            return allRole;
        }

        public IEnumerable<SettingRelation> GetAllResponses()
        {
            var allResponse = CacheService.Get<IEnumerable<SettingRelation>>(GetAllResponseSessionKey);
            if (allResponse != null)
            {
                return allResponse;
            }

            allResponse = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Response,
                ParentId = 0
            });

            CacheService.Set(GetAllResponseSessionKey, allResponse);
            return allResponse;
        }

        public IEnumerable<Setting> GetAllCustomerTypes()
        {
            return new[]
            {
                new Setting
                {
                    Id = CustomerTypeConstants.All,
                    Value = Labels.All
                },
                new Setting
                {
                    Id = CustomerTypeConstants.Customer,
                    Value = Labels.Customer
                },
                new Setting
                {
                    Id = CustomerTypeConstants.Guest,
                    Value = Labels.Guest
                }
            };
        }

        public IEnumerable<Setting> GetAllVIPTypes()
        {
            return new[]
            {
                new Setting
                {
                    Id = VIPTypeConstants.All,
                    Value = Labels.All
                },
                new Setting
                {
                    Id = VIPTypeConstants.Yes,
                    Value = Labels.Yes
                },
                new Setting
                {
                    Id = VIPTypeConstants.No,
                    Value = Labels.No
                }
            };
        }

        public IEnumerable<Setting> GetPositions()
        {
            return new[]
            {
                new Setting { Id = RoleIdConstants.Super, Value = Labels.Super },
                new Setting { Id = RoleIdConstants.Master, Value = Labels.Master },
                new Setting { Id = RoleIdConstants.Agent, Value = Labels.Agent },
                new Setting { Id = RoleIdConstants.Member, Value = Labels.Member },
                new Setting { Id = RoleIdConstants.Guest, Value = Labels.Guest }
            };
        }

        public IEnumerable<Setting> AllHeadlineType()
        {
            return new List<Setting>()
            {
                new Setting
                {
                    Id = HeadlineTypeConstants.All,
                    Value = Labels.All
                },
                new Setting
                {
                    Id = HeadlineTypeConstants.Yes,
                    Value = Labels.Yes
                },
                new Setting
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
        /// <param name="allSettingRelations">All setting relation.</param>
        /// <param name="allSettings">All setting.</param>
        /// <returns>The sub setting by parent identifier.</returns>
        public IEnumerable<Setting> GetSubSettingsByParentId(
            BaseModel parentSetting,
            IEnumerable<Setting> allSettingRelations,
            IEnumerable<Setting> allSettings)
        {
            var subSettings = new List<Setting>();
            if (allSettingRelations == null || !allSettingRelations.Any())
            {
                return subSettings;
            }

            foreach (var settingRelation in allSettingRelations)
            {
                if (settingRelation.ParentId == parentSetting.Id)
                {
                    var settings = allSettings
                        .Where(x => x.Id == settingRelation.Id)
                        .Select(x => new Setting
                        {
                            Id = x.Id,
                            DisplayOrder = x.DisplayOrder,
                            Value = x.Value,
                            SettingType = x.SettingType,
                            ParentId = parentSetting.Id,
                            IsChild = true
                        });

                    subSettings.AddRange(settings);
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

        public IEnumerable<Setting> GetAllSiteTypes()
        {
            return new List<Setting>()
            {
                new Setting { Id = SiteConstants.All, Value = Labels.All },
                new Setting { Id = SiteConstants.NA, Value = Labels.Symbol_NotApply },
                new Setting { Id = SiteConstants.IBCGroup, Value = Labels.Credit },
                new Setting { Id = SiteConstants.LicenseeGroup, Value = Labels.Licensee },
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

        public IEnumerable<Setting> GetMainSettings()
        {
            var allSettings = GetAllSettings();
            var settingGroups = allSettings
                .GroupBy(x => new { Type = x.SettingType })
                .Select(group => new { group.Key, Count = group.Count() });
            var settingGroupByType = new List<Setting>();
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

        private void GetSubSetting(string key, IList<Setting> settingList, IEnumerable<Setting> allSettings)
        {
            var subList = allSettings.Where(s => s.SettingType == key).Select(s => s).OrderBy(s => s.DisplayOrder).ToList();
            var setting = new Setting { SettingType = key, ChildSettings = subList };
            settingList.Add(setting);
        }

        private IEnumerable<T> GetSettingRelationByName<T>(GetSettingRelationByNameCriteria criteria) where T : SettingRelation, new()
        {
            var settingRelations = _settingRepositoryInstance.GetSettingRelations<T>(criteria);

            return settingRelations.OrderBy(setting => setting.DisplayOrder).ToList();
        }

        private IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplates()
        {
            var feedbackTemplates = _settingRepositoryInstance.GetFeedbackSubjectTemplateById(new GetFeedbackSubjectTemplateByIdCriteria
            {
                TemplateId = 0
            }).Where(s => s.IsActive);

            CacheService.Set(GetAllFeedbackTemplateSubjectSessionKey, feedbackTemplates);
            return feedbackTemplates;
        }
    }
}