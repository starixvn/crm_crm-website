namespace Fanex.CRM.Setting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Common.Constants;
    using Fanex.CRM.Common.Extensions;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories;
    using Fanex.CRM.Setting.Resources;

    public class SettingService : ISettingService
    {
        public const string GetAllSettingSessionKey = "Fanex.CRM.Setting.GetAllSettingSessionKey";
        public const string GetAllCountrySessionKey = "Fanex.CRM.Setting.GetAllCountrySessionKey";
        public const string GetAllTitleSessionKey = "Fanex.CRM.Setting.GetAllTitleSessionKey";
        public const string GetAllFeedbackTemplateSubjectSessionKey = "Fanex.CRM.Setting.GetAllFeedbackTemplateSubjectSessionKey";

        public const string GetInformHeadlineStatusesSessionKey = "Fanex.CRM.Setting.GetInformHeadlineStatusesSessionKey";
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
        private readonly IBaseService baseService;

        public SettingService() : this(null, null)
        {
        }

        public void Delete(string key)
        {
            RedisCacheService.Delete(key);
        }

        public void ClearAllCaches(string key= "")
        {
            var SessionKeys = new List<string>
            {
                GetAllSettingSessionKey, GetAllCountrySessionKey, GetAllTitleSessionKey,GetAllFeedbackTemplateSubjectSessionKey,
                GetInformHeadlineStatusesSessionKey, GetAllLanguageSessionKey,
                GetAllExtensionSessionKey, GetAllCommunicationToolSessionKey, GetAllMainCategorySessionKey, GetAllMainCategoryFromAllSettingsSessionKey,
                GetProductOnlySessionKey, GetAllProductSessionKey, GetAllSubProductSessionKey, GetAllStatusSessionKey, GetAllStatusCommunicationTool,
                GetAllStatusRedemption,GetAllRoleSessionKey, GetAllResponseSessionKey, GetAllActivityLogActionKey,GetAllSettingLogActionKey,
                GetAllInformHeadlineSettingKey, GetAllLicenseeTypeSessionKey, GetAllCurrencySessionKey, GetAllCurrencyRedemptionSessionKey,
                GetProductsSessionKey, GetSubProductsSessionKey, GetBettypesSessionKey, GetMainCategorySessionKey, GetSubCategorySessionKey
            };

            if(!string.IsNullOrEmpty(key))
            {
                SessionKeys.Add(key);
            }    

            RedisCacheService.ClearCache(SessionKeys);
        }

        public SettingService(ISettingRepository settingRepositoryInstance, IBaseService baseService)
        {
            _settingRepositoryInstance = settingRepositoryInstance ?? new SettingRepository();
            this.baseService = baseService ?? new BaseService();
        }

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

        public Setting GetSettingById(GetSettingByIdCriteria criteria)
        {
            var setting = GetAllSettings().FirstOrDefault(s => s.Id == criteria.SettingId);
            return setting ?? new Setting
            {
                Value = Labels.Symbol_NotApply
            };
        }

        public List<Setting> GetAllSettings()
        {
            var allSettings = RedisCacheService.Get<List<Setting>>(GetAllSettingSessionKey);
            if (allSettings == null)
            {
                allSettings = _settingRepositoryInstance.GetAllSettings(new GetAllSettingCriteria());
                RedisCacheService.Set(GetAllSettingSessionKey, allSettings);
            }
            return allSettings;
        }

        public List<Setting> GetRolesByPic(int picType)
        {
            var allSettings = _settingRepositoryInstance.GetRoleListByPic(new GetRoleByPicType{ PicType = picType });
            return allSettings;
        }

	    public List<Setting> GetAllSettingsRemoveCache()
        {
            var  allSettings = _settingRepositoryInstance.GetAllSettings(new GetAllSettingCriteria());
            return allSettings;
        }

        public IList<SiteInfo> GetAllSites(GetAllSiteCriteria criteria = null)
        {
           var sites = baseService.GetData<SiteInfo>(criteria ?? new GetAllSiteCriteria());
           var allSites = (from n in sites
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

            return allSites;
        }

        public IEnumerable<SiteInfo> GetAllSitesLicenseeInactive(GetAllSiteCriteria criteria = null)
        {
            var sites = baseService.GetData<SiteInfo>(criteria ?? new GetAllSiteCriteria());

            var allSites = sites.Where(x => !x.IsActive && x.IsLicensee);

            return allSites;
        }

        public IList<SiteInfo> GetAllSitesCN88()
        {
            var allSites = new List<SiteInfo>
            {
                new SiteInfo
                {
                    Id = 1,
                    Name = "CN88",
                    ChildSites = new List<SiteInfo> {
                        new SiteInfo
                        {
                            Id = 33002,
                            Name = "CN88"
                        }
                    }
                }
            };

            return allSites;
        }

        public IList<SiteSetting> GetAllSiteWithGroupCN88()
        {
            return new List<SiteSetting>
            {
                new SiteSetting
                {
                    Id = 33002,
                    Name = "CN88",
                    AliasName = "CN88"
                }
            };
        }

        public IList<SiteSetting> GetAllSiteWithGroup()
        {
            var sites = new List<SiteSetting>();
            var rawdata = baseService.GetData<SiteInfo>(new GetAllSiteCriteria());
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
            return sites;
        }

        public IList<SettingRelation> GetAllTitles()
        {
            var cache = RedisCacheService.Get<IList<SettingRelation>>(GetAllTitleSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var allTitles = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Title,
                ParentId = 0,
                Active = SettingConstants.ActiveAndInactive
            });

            RedisCacheService.Set(GetAllTitleSessionKey, allTitles);

            return allTitles;
        }

        public IList<Country> GetAllCountries()
        {
            var allCountry = RedisCacheService.Get<IList<Country>>(GetAllCountrySessionKey);
            if (allCountry != null)
            {
                return allCountry;
            }

            allCountry = _settingRepositoryInstance.GetAllCountries(new GetAllCountryCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });
            RedisCacheService.Set(GetAllCountrySessionKey, allCountry);
            return allCountry;
        }

        public IList<Province> GetAllProvinces()
        {
            return _settingRepositoryInstance.GetAllProvinces(new GetAllProvinceCriteria());
        }

        public IList<Setting> GetAllLanguages()
        {
            var allLanguages = RedisCacheService.Get<IList<Setting>>(GetAllLanguageSessionKey);
            if (allLanguages != null)
            {
                return allLanguages;
            }

            allLanguages = _settingRepositoryInstance.GetAllLanguages(new GetAllLanguageCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });
            RedisCacheService.Set(GetAllLanguageSessionKey, allLanguages);
            return allLanguages;
        }

        public IList<SettingRelation> GetAllExtensions()
        {
            var allExtensions = RedisCacheService.Get<IList<SettingRelation>>(GetAllExtensionSessionKey);
            if (allExtensions != null)
            {
                return allExtensions;
            }

            allExtensions = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Extension,
                ParentId = 0,
                Active = SettingConstants.Active
            })
            .Where(e => e.Id != NAConstants.GetExtentionNA(AppSession.CurrentUser.UserRole))
            .ToList();

            RedisCacheService.Set(GetAllExtensionSessionKey, allExtensions);
            return allExtensions;
        }

        public SettingRelation GetExtensionNA()
        {
            var allExtensions = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Extension,
                ParentId = 0,
                Active = SettingConstants.Active
            });

            return allExtensions.FirstOrDefault(e => e.Id == NAConstants.GetExtentionNA(AppSession.CurrentUser.UserRole));
        }

        public IList<Setting> GetAllCommunicationTools(bool isIncludeNA = false)
        {
            var tools = RedisCacheService.Get<IList<Setting>>(GetAllCommunicationToolSessionKey);
            if (tools == null)
            {
                tools = _settingRepositoryInstance.GetAllCommunicationTools(new GetAllCommunicationToolCriteria
                {
                    Active = SettingConstants.ActiveAndInactive
                });
                RedisCacheService.Set(GetAllCommunicationToolSessionKey, tools);
            }

            return isIncludeNA
                ? tools.OrderBy(s => s.DisplayOrder).ToList()
                : tools.Where(t => t.Id != NAConstants.GetCommunicationNA(AppSession.CurrentUser.UserRole)).OrderBy(s => s.DisplayOrder).ToList();
        }

        public Product GetProductNA()
        {
            var products = GetSettingRelationByName<Product>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Product,
                ParentId = 0,
                Active = SettingConstants.Active
            });

            return products.FirstOrDefault(p => p.Id == NAConstants.GetProductNA(AppSession.CurrentUser.UserRole));
        }

        public Product GetSubProductNA()
        {
            return _settingRepositoryInstance
                .GetSubProducts(new SubProductCriteria
                {
                    Active = SettingConstants.Active
                })
                .FirstOrDefault(s => s.Id == NAConstants.GetSubProductNA(AppSession.CurrentUser.UserRole));
        }

        public Product GetBettypeNA()
        {
            return _settingRepositoryInstance
                    .GetAllBetType(new GetAllBetTypeCriteria
                    {
                        Active = SettingConstants.Active
                    })
                    .FirstOrDefault(b => b.Id == NAConstants.GetBettypeNA(AppSession.CurrentUser.UserRole));
        }

        public IList<Setting> GetAllMainCategories()
        {
            var cache = RedisCacheService.Get<IList<Setting>>(GetAllMainCategorySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var mainCategories = _settingRepositoryInstance.GetAllMainCategory(new GetAllMainCategoryCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });
            var subCategories = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });

            foreach (var category in mainCategories)
            {
                category.ChildSettings = subCategories.Where(x => x.ParentId == category.Id).OrderBy(s => s.DisplayOrder).ToList();
            }

            RedisCacheService.Set(GetAllMainCategorySessionKey, mainCategories);
            return mainCategories;
        }

        public IList<Setting> GetMainCategories()
        {
            var cache = RedisCacheService.Get<IList<Setting>>(GetMainCategorySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var mainCategories = _settingRepositoryInstance.GetAllMainCategory(new GetAllMainCategoryCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });

            RedisCacheService.Set(GetMainCategorySessionKey, mainCategories);
            return mainCategories;
        }

        public IList<Setting> GetSubCategories()
        {
            var cache = RedisCacheService.Get<IList<Setting>>(GetSubCategorySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var subCategories = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });

            RedisCacheService.Set(GetSubCategorySessionKey, subCategories);
            return subCategories;
        }

        public IList<Setting> GetAllMainCategoriesFromAllSettings()
        {
            var cache = RedisCacheService.Get<IList<Setting>>(GetAllMainCategoryFromAllSettingsSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var mainCategories = GetAllSettings().Where(s => s.SettingType.Equals(SettingTypeConstants.MainCategory)).ToList();
            var subCategories = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });

            foreach (var category in mainCategories)
            {
                category.ChildSettings = subCategories.Where(x => x.ParentId == category.Id).OrderBy(s => s.DisplayOrder).ToList();
            }

            RedisCacheService.Set(GetAllMainCategoryFromAllSettingsSessionKey, mainCategories);
            return mainCategories;
        }

        public IList<Product> GetProductsOnly()
        {
            var cache = RedisCacheService.Get<IList<Product>>(GetProductOnlySessionKey);
            if (cache != null)
            {
                return cache;
            }

            var products = GetSettingRelationByName<Product>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Product,
                ParentId = 0,
                Active = SettingConstants.ActiveAndInactive
            });

            RedisCacheService.Set(GetProductOnlySessionKey, products);

            return products;
        }

        public IList<Product> GetAllProducts()
        {
            var cache = RedisCacheService.Get<IList<Product>>(GetAllProductSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var parentProducts = GetProductsOnly();
            var subProducts = GetAllSubProducts();
            var products = parentProducts
                        .Where(p => p.Id != NAConstants.GetProductNA(AppSession.CurrentUser.UserRole))
                        .Select(p =>
                        {
                            p.Name = p.Value;
                            p.ChildSettings = subProducts.Where(s => s.ParentId == p.Id).ToList();
                            return p;
                        })
                        .ToList();

            RedisCacheService.Set(GetAllProductSessionKey, products);

            return products;
        }

        public IList<Product> GetAllSubProducts()
        {
            var cache = RedisCacheService.Get<IList<Product>>(GetAllSubProductSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var subProducts = _settingRepositoryInstance.GetSubProducts(new SubProductCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });
            var betTypes = _settingRepositoryInstance.GetAllBetType(new GetAllBetTypeCriteria
            {
                Active = SettingConstants.ActiveAndInactive
            });

            var products = subProducts
                        .Where(sp => sp.Id != NAConstants.GetSubProductNA(AppSession.CurrentUser.UserRole))
                        .Select(sp =>
                        {
                            sp.Name = sp.Value;
                            sp.ChildSettings = betTypes
                                    .Where(s => s.ParentId == sp.Id && s.Id != NAConstants.GetBettypeNA(AppSession.CurrentUser.UserRole))
                                    .Select(s =>
                                    {
                                        s.GrandParentId = sp.ParentId;
                                        return s;
                                    }).OrderBy(s => s.DisplayOrder).ToList();

                            return sp;
                        })
                        .ToList();

            RedisCacheService.Set(GetAllSubProductSessionKey, products);
            return products;
        }

        public IList<Product> GetProducts()
        {
            var cache = RedisCacheService.Get<IList<Product>>(GetProductsSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var products = GetProductsOnly()
                              .Where(p => p.Id != NAConstants.GetProductNA(AppSession.CurrentUser.UserRole))
                              .Select(p =>
                              {
                                  p.Name = p.Value;
                                  return p;
                              }).ToList();

            RedisCacheService.Set(GetProductsSessionKey, products);
            return products;
        }

        public IList<Product> GetSubProducts()
        {
            var cache = RedisCacheService.Get<IList<Product>>(GetSubProductsSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var subProducts = _settingRepositoryInstance
                                .GetSubProducts(new SubProductCriteria
                                {
                                    Active = SettingConstants.ActiveAndInactive
                                })
                                .Where(p => p.Id != NAConstants.GetSubProductNA(AppSession.CurrentUser.UserRole) && p.ParentId != 0)
                                .Select(p =>
                                {
                                    p.Name = p.Value;
                                    return p;
                                }).ToList();

            RedisCacheService.Set(GetSubProductsSessionKey, subProducts);
            return subProducts;
        }

        public IList<Product> GetBettypes()
        {
            var cache = RedisCacheService.Get<IList<Product>>(GetBettypesSessionKey);
            if (cache != null)
            {
                return cache;
            }

            var betTypes = _settingRepositoryInstance
                               .GetAllBetType(new GetAllBetTypeCriteria
                               {
                                   Active = SettingConstants.ActiveAndInactive
                               })
                               .Where(b => b.Id != NAConstants.GetBettypeNA(AppSession.CurrentUser.UserRole))
                               .OrderBy(b => b.DisplayOrder)
                               .ToList();

            RedisCacheService.Set(GetBettypesSessionKey, betTypes);
            return betTypes;
        }

        public List<BaseModel> GetAllStatuses(int statusType)
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

            var allStatus = RedisCacheService.Get<List<BaseModel>>(key);
            if (allStatus != null)
            {
                return allStatus;
            }

            allStatus = _settingRepositoryInstance.GetAllStatus(new GetAllStatusCriteria(statusType)).ToList();
            RedisCacheService.Set(key, allStatus);

            return allStatus;
        }

        public IList<Status> GetInformHeadlineStatuses()
        {
            const string key = GetInformHeadlineStatusesSessionKey;
            var allStatus = RedisCacheService.Get<IList<Status>>(key);
            if (allStatus != null)
            {
                return allStatus;
            }

            allStatus = baseService.GetData<Status>(new GetAllStatusCriteria(ViewTypeConstants.InformHeadline))
                .OrderBy(s => s.DisplayOrder)
                .ToList();
            RedisCacheService.Set(key, allStatus);

            return allStatus;
        }

        public List<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria)
        {
            const string key = GetAllCurrencyRedemptionSessionKey;
            var currencies = RedisCacheService.Get<List<BaseModel>>(key);
            if (currencies == null)
            {
                currencies = _settingRepositoryInstance.GetAllCurrencyRedemptions(criteria).ToList();
                RedisCacheService.Set(key, currencies);
            }

            return currencies;
        }

        public IList<SettingRelation> GetAllRoles()
        {
            var allRole = RedisCacheService.Get<IList<SettingRelation>>(GetAllRoleSessionKey);
            if (allRole != null)
            {
                return allRole;
            }

            allRole = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Role,
                ParentId = 0,
                Active = SettingConstants.ActiveAndInactive
            });

            RedisCacheService.Set(GetAllRoleSessionKey, allRole);
            return allRole;
        }

        public IList<SettingRelation> GetAllResponses()
        {
            var allResponse = RedisCacheService.Get<IList<SettingRelation>>(GetAllResponseSessionKey);
            if (allResponse != null)
            {
                return allResponse;
            }

            allResponse = GetSettingRelationByName<SettingRelation>(new GetSettingRelationByNameCriteria
            {
                SettingName = SettingTypeConstants.Response,
                ParentId = 0,
                Active = SettingConstants.ActiveAndInactive
            });

            RedisCacheService.Set(GetAllResponseSessionKey, allResponse);
            return allResponse;
        }

        public IList<Setting> GetAllCustomerTypes()
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

        public IList<Setting> GetAllVIPTypes()
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

        public IList<Setting> GetPositions(bool isExcludeGuest)
        {
            if (isExcludeGuest)
            {
                return new[]
                     {
                        new Setting { Id = RoleIdConstants.Super, Value = Labels.Super },
                        new Setting { Id = RoleIdConstants.Master, Value = Labels.Master },
                        new Setting { Id = RoleIdConstants.Agent, Value = Labels.Agent },
                        new Setting { Id = RoleIdConstants.Member, Value = Labels.Member }
                     };
            }
            return new[]
            {
                new Setting { Id = RoleIdConstants.Super, Value = Labels.Super },
                new Setting { Id = RoleIdConstants.Master, Value = Labels.Master },
                new Setting { Id = RoleIdConstants.Agent, Value = Labels.Agent },
                new Setting { Id = RoleIdConstants.Member, Value = Labels.Member },
                new Setting { Id = RoleIdConstants.Guest, Value = Labels.Guest}
            };
        }

        public IList<Setting> GetPositionsCN88()
        {
            return new[]
            {
                new Setting { Id = -2, Value = Labels.Headline },
                new Setting { Id = RoleIdConstants.Super, Value = Labels.Super },
                new Setting { Id = RoleIdConstants.Master, Value = Labels.Master },
                new Setting { Id = RoleIdConstants.Agent, Value = Labels.Agent },
                new Setting { Id = RoleIdConstants.Member, Value = Labels.Member },
                new Setting { Id = RoleIdConstants.Guest, Value = Labels.Guest }
            };
        }

        public IList<Setting> AllHeadlineType()
        {
            return new List<Setting>
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

        public IList<BaseModel> GetAllLicenseeTypes()
        {
            const string key = GetAllLicenseeTypeSessionKey;
            var cache = RedisCacheService.Get<IList<BaseModel>>(key);
            if (cache != null)
            {
                return cache;
            }

            var licenseeTypes = _settingRepositoryInstance.GetAllLicenseeTypes(new GetAllLicenseeTypeCriteria());
            licenseeTypes.Insert(
                0,
                new BaseModel
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.Symbol_NotApply
                });

            RedisCacheService.Set(key, licenseeTypes);
            return licenseeTypes;
        }

        public IList<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria, bool hasDefaultOption = true)
        {
            const string key = GetAllCurrencySessionKey;
            var cache = RedisCacheService.Get<IList<BaseModel>>(key);
            if (cache != null)
            {
                return cache;
            }

            var currencies = _settingRepositoryInstance.GetAllCurrencies(criteria);

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

            RedisCacheService.Set(key, currencies);

            return currencies;
        }

        public int NumberOfFeedbackSubjectTemplate()
        {
            var feedbackTemplates = GetFeedbackSubjectTemplates();
            return feedbackTemplates.Count();
        }

        public IList<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            int multipleNum = SettingConstants.MaxFeedbackSubjectTemplateNumberOnLoad;
            var feedbackTemplates = RedisCacheService.Get<IList<FeedbackTemplate>>(GetAllFeedbackTemplateSubjectSessionKey);

            if (feedbackTemplates == null)
            {
                feedbackTemplates = GetFeedbackSubjectTemplates();
            }

            return (feedbackTemplates != null && feedbackTemplates.Any()) ?
                        feedbackTemplates.Skip((criteria.PagingIndex - 1) * multipleNum).Take(criteria.PagingIndex * multipleNum).ToList() :
                        new List<FeedbackTemplate>();
        }

        public IList<BaseModel> GetAllLogActions(GetActionCriteria criteria)
        {
            string cachedKey = criteria.ActionType == SettingConstants.ActivityLog
                ? GetAllActivityLogActionKey
                : GetAllSettingLogActionKey;
            var actions = RedisCacheService.Get<IList<BaseModel>>(cachedKey);
            if (actions != null)
            {
                return actions;
            }

            actions = _settingRepositoryInstance.GetAllLogActions(criteria);
            RedisCacheService.Set(cachedKey, actions);
            return actions;
        }

        public IList<Setting> GetAllSiteTypes()
        {
            return new List<Setting>
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
                const string cachedKey = GetAllInformHeadlineSettingKey;
                var settings = RedisCacheService.Get<InformHeadlineSetting>(cachedKey);
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
                    ProductStatus = new List<BaseModel>
                    {
                        new BaseModel { Id = HeadlineProductStatusConstants.NotAvailable, Name = Labels.Symbol_NotApply },
                        new BaseModel { Id = HeadlineProductStatusConstants.Enable, Name = Labels.Enable },
                        new BaseModel { Id = HeadlineProductStatusConstants.Disable, Name = Labels.Disable }
                    },
                    Extensions = GetAllExtensions()
                };

                RedisCacheService.Set(cachedKey, settings);
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
            return new[] {
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

        public BaseModel[] GetFeedbackStatisticFilterTypesCN88()
        {
            return new[]
            {
                new BaseModel { Id = FeedbackStatisticFilterType.ByCreator, Name = Labels.ByCreator },
                new BaseModel { Id = FeedbackStatisticFilterType.ByRole, Name = Labels.ByRole },
                new BaseModel { Id = FeedbackStatisticFilterType.ByStatus, Name = Labels.ByStatus },
                new BaseModel { Id = FeedbackStatisticFilterType.ByResponse, Name = Labels.ByResponse },
                new BaseModel { Id = FeedbackStatisticFilterType.ByProduct, Name = Labels.ByProduct },
                new BaseModel { Id = FeedbackStatisticFilterType.ByCategory, Name = Labels.ByCategory },
                new BaseModel { Id = FeedbackStatisticFilterType.ByCommunicationTool, Name = Labels.ByCommunicationTool },
                new BaseModel { Id = FeedbackStatisticFilterType.ByProvince, Name = Labels.ByProvince },
            };
        }

        public IList<Setting> GetMainSettings()
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

        public IList<Station> GetStations()
        {
            return baseService.GetData<Station>(new GetStationsCriteria()).ToList();
        }

        private void GetSubSetting(string key, IList<Setting> settingList, IEnumerable<Setting> allSettings)
        {
            var subList = allSettings.Where(s => s.SettingType == key).Select(s => s).OrderBy(s => s.DisplayOrder).ToList();
            var setting = new Setting { SettingType = key, ChildSettings = subList };
            settingList.Add(setting);
        }

        private IList<T> GetSettingRelationByName<T>(GetSettingRelationByNameCriteria criteria) where T : SettingRelation, new()
        {
            var settingRelations = _settingRepositoryInstance.GetSettingRelations<T>(criteria);

            return settingRelations.OrderBy(setting => setting.DisplayOrder).ToList();
        }

        private IList<FeedbackTemplate> GetFeedbackSubjectTemplates()
        {
            var feedbackTemplates = baseService.GetData<FeedbackTemplate>(new GetFeedbackSubjectTemplateByIdCriteria
            {
                TemplateId = 0
            }).Where(s => s.IsActive).ToList();

            RedisCacheService.Set(GetAllFeedbackTemplateSubjectSessionKey, feedbackTemplates);
            return feedbackTemplates;
        }

        public IList<SiteInfo> GetAllSitesWithNA()
        {
            var sites = GetAllSites();
            sites.Insert(0, new SiteInfo { Id = -1, Name = Labels.Symbol_NotApply, IsActive = true });

            return sites;
        }

        public IList<Setting> GetPicSettings(string settingName, bool? isActive = null, bool isIncludeAllOption = false)
        {
            var data = GetAllSettings().Where(s => s.SettingType.Equals(settingName) && (isActive == null || s.IsActive == isActive.Value)).ToList();

            if (isIncludeAllOption)
            {
                data.Insert(0, new Setting
                {
                    Id = 0,
                    Value = Labels.All
                });
            }

            return data;
        }

        public IEnumerable<SettingMapping> GetSettingMappings(GetSourceSettingMappingCriteria criteria)
        {
            return _settingRepositoryInstance.GetSettingMappings(criteria);
        }
    }
}