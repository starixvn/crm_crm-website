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
        public SettingInfo SettingById(GetSettingByIdCriteria criteria)
        {
            try
            {
                SettingInfo setting = null;
                var settings = AllSetting();
                setting = settings.Where(s => s.Id == criteria.SettingId).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingInfo()
                    {
                        Value = Labels.Symbol_NotApply
                    };
                }

                return setting;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        public List<SettingInfo> AllSetting()
        {
            try
            {
                List<SettingInfo> allSetting = CacheService.Get<List<SettingInfo>>(GetAllSettingSessionKey);
                if (allSetting == null)
                {
                    allSetting = _settingRepositoryInstance.GetAllSetting(new GetAllSettingCriteria());
                    CacheService.Set(GetAllSettingSessionKey, allSetting);
                }

                return allSetting;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <param name="criteria">Get all site criteria.</param>
        /// <returns>All site value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<SiteInfo> AllSite(GetAllSiteCriteria criteria = null)
        {
            try
            {
                var allSite = new List<SiteInfo>();
                allSite = CacheService.Get<List<SiteInfo>>(GetAllSiteSessionKey);
                if (allSite != null)
                {
                    return allSite;
                }

                allSite = new List<SiteInfo>();
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all site with group.
        /// </summary>
        /// <returns>All site value.</returns>
        public IList<BaseModel> AllSiteWithGroup()
        {
            try
            {
                string key = GetAllSiteWithGroupSessionKey;
                var sites = CacheService.Get<IList<BaseModel>>(key);
                if (sites == null)
                {
                    string group;
                    sites = new List<BaseModel>();
                    var rawdata = GetDynamicData<SiteInfo>(new GetAllSiteCriteria());
                    foreach (var item in rawdata)
                    {
                        group = item.IsLicensee ? Labels.Licensee : Labels.Credit;
                        sites.Add(new BaseModel { Id = item.Id, Name = item.Name, AliasName = group });
                    }

                    CacheService.Set(key, sites);
                }

                return sites;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all title.
        /// </summary>
        /// <returns>All title.</returns>
        public List<SettingInfo> AllTitle()
        {
            try
            {
                var allTitle = new List<SettingInfo>();
                allTitle = CacheService.Get<List<SettingInfo>>(GetAllTitleSessionKey);
                if (allTitle != null)
                {
                    return allTitle;
                }

                allTitle = new List<SettingInfo>();
                allTitle = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Title,
                    ParentId = 0
                });

                CacheService.Set(GetAllTitleSessionKey, allTitle);
                return allTitle;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <returns>All country.</returns>
        public List<CountryInfo> AllCountry()
        {
            try
            {
                var allCountry = new List<CountryInfo>();
                allCountry = CacheService.Get<List<CountryInfo>>(GetAllCountrySessionKey);
                if (allCountry != null)
                {
                    return allCountry;
                }

                allCountry = _settingRepositoryInstance.GetAllCountry(new GetAllCountryCriteria());
                CacheService.Set(GetAllCountrySessionKey, allCountry);
                return allCountry;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all language.
        /// </summary>
        /// <returns>All language.</returns>
        public List<SettingInfo> AllLanguage()
        {
            try
            {
                var allLanguage = new List<SettingInfo>();
                allLanguage = CacheService.Get<List<SettingInfo>>(GetAllLanguageSessionKey);
                if (allLanguage != null)
                {
                    return allLanguage;
                }

                allLanguage = _settingRepositoryInstance.GetAllLanguage(new GetAllLanguageCriteria());
                CacheService.Set(GetAllLanguageSessionKey, allLanguage);
                return allLanguage;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all extension.
        /// </summary>
        /// <returns>All extension.</returns>
        public List<SettingInfo> AllExtension()
        {
            try
            {
                var allExtension = new List<SettingInfo>();
                allExtension = CacheService.Get<List<SettingInfo>>(GetAllExtensionSessionKey);
                if (allExtension != null)
                {
                    return allExtension;
                }

                allExtension = new List<SettingInfo>();
                allExtension = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Extension,
                    ParentId = 0
                });

                CacheService.Set(GetAllExtensionSessionKey, allExtension);
                return allExtension;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> AllCommunicationTool()
        {
            try
            {
                var allCommunicationTool = new List<SettingInfo>();
                allCommunicationTool = CacheService.Get<List<SettingInfo>>(GetAllCommunicationToolSessionKey);
                if (allCommunicationTool != null)
                {
                    return allCommunicationTool;
                }

                allCommunicationTool = _settingRepositoryInstance.GetAllCommunicationTool(new GetAllCommunicationToolCriteria());
                allCommunicationTool = allCommunicationTool.OrderBy(s => s.DisplayOrder).ToList();
                CacheService.Set(GetAllCommunicationToolSessionKey, allCommunicationTool);

                return allCommunicationTool;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> AllMainCategory()
        {
            try
            {
                var allMainCategory = new List<SettingInfo>();
                allMainCategory = CacheService.Get<List<SettingInfo>>(GetAllMainCategorySessionKey);
                if (allMainCategory != null)
                {
                    return allMainCategory;
                }

                allMainCategory = new List<SettingInfo>();
                var rawData = _settingRepositoryInstance.GetAllMainCategory(new GetAllMainCategoryCriteria());
                foreach (var data in rawData)
                {
                    data.ChildSettings = _settingRepositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria
                    {
                        ParentId = data.Id
                    });

                    data.ChildSettings = data.ChildSettings.OrderBy(s => s.DisplayOrder).ToList();
                    allMainCategory.Add(data);
                }

                CacheService.Set(GetAllMainCategorySessionKey, allMainCategory);
                return allMainCategory;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<SettingInfo> AllProduct()
        {
            try
            {
                var allProduct = new List<SettingInfo>();
                allProduct = CacheService.Get<List<SettingInfo>>(GetAllProductSessionKey);
                if (allProduct != null)
                {
                    return allProduct;
                }

                allProduct = new List<SettingInfo>();
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

                allProduct.Add(new SettingInfo
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
                    });

                    data.ChildSettings = data.ChildSettings.OrderBy(s => s.DisplayOrder).ToList();
                    data.ChildSettings.Insert(0, notApplySetting);
                    allProduct.Add(data);
                }

                CacheService.Set(GetAllProductSessionKey, allProduct);
                return allProduct;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns>
        /// All status.
        /// </returns>
        public List<SettingInfo> AllStatus(int statusType)
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

            try
            {
                var allStatus = CacheService.Get<List<SettingInfo>>(key);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="statusType">Type of the status.</param>
        /// <returns>
        /// All status.
        /// </returns>
        public IList<Status> GetInformHeadlineStatuses()
        {
            string key = GetInformHeadlineStatusesSessionKey;

            try
            {
                var allStatus = CacheService.Get<IList<Status>>(key);
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// All the currency redemption.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Setting Info List.</returns>
        public List<BaseModel> AllCurrencyRedemption(GetAllCurrencyRedemptionCriteria criteria)
        {
            string key = GetAllCurrencyRedemptionSessionKey;
            try
            {
                var currencies = CacheService.Get<List<BaseModel>>(key);
                if (currencies == null)
                {
                    currencies = _settingRepositoryInstance.GetAllCurrencyRedemption(criteria);
                    CacheService.Set(key, currencies);
                }

                return currencies;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all role.
        /// </summary>
        /// <returns>All role value.</returns>
        public List<SettingInfo> AllRole()
        {
            try
            {
                var allRole = CacheService.Get<List<SettingInfo>>(GetAllRoleSessionKey);
                if (allRole != null)
                {
                    return allRole;
                }

                allRole = new List<SettingInfo>();
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Role,
                    ParentId = 0
                });

                allRole = rawData;
                CacheService.Set(GetAllRoleSessionKey, allRole);
                return allRole;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all response.
        /// </summary>
        /// <returns>All response.</returns>
        public List<SettingInfo> AllResponse()
        {
            try
            {
                var allResponse = new List<SettingInfo>();
                allResponse = CacheService.Get<List<SettingInfo>>(GetAllResponseSessionKey);
                if (allResponse != null)
                {
                    return allResponse;
                }

                allResponse = new List<SettingInfo>();
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Response,
                    ParentId = 0
                });

                allResponse = rawData;
                CacheService.Set(GetAllResponseSessionKey, allResponse);
                return allResponse;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all customer type.
        /// </summary>
        /// <returns>All customer type.</returns>
        public List<SettingInfo> AllCustomerType()
        {
            try
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all VIP type.
        /// </summary>
        /// <returns>All VIP type.</returns>
        public List<SettingInfo> AllVIPType()
        {
            try
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the type of all headline.
        /// </summary>
        /// <returns>The list of headline type.</returns>
        public List<SettingInfo> AllHeadlineType()
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

        /// <summary>
        /// Gets all licensee types.
        /// </summary>
        /// <returns>The all licensee types.</returns>
        public List<BaseModel> AllLicenseeType()
        {
            string key = GetAllLicenseeTypeSessionKey;
            try
            {
                var licenseeTypes = CacheService.Get<List<BaseModel>>(key);
                if (licenseeTypes != null)
                {
                    return licenseeTypes;
                }

                licenseeTypes = _settingRepositoryInstance.GetAllLicenseeType(new GetAllLicenseeTypeCriteria());
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all currency.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="hasDefaultOption">Has N/A option or not.</param>
        /// <returns>The currency list.</returns>
        public List<BaseModel> GetAllCurrency(GetAllCurrencyCriteria criteria, bool hasDefaultOption = true)
        {
            string key = GetAllCurrencySessionKey;
            try
            {
                var currencies = CacheService.Get<List<BaseModel>>(key);
                if (currencies == null)
                {
                    currencies = _settingRepositoryInstance.GetAllCurrency(criteria);
                    CacheService.Set(key, currencies);
                }

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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the sub setting by parent identifier.
        /// </summary>
        /// <param name="parentSetting">The parent setting.</param>
        /// <param name="allSettingRelation">All setting relation.</param>
        /// <param name="allSetting">All setting.</param>
        /// <returns>The sub setting by parent identifier.</returns>
        public List<SettingInfo> GetSubSettingByParentId(
            BaseModel parentSetting,
            List<SettingInfo> allSettingRelation,
            List<SettingInfo> allSetting)
        {
            try
            {
                List<SettingInfo> subSettings = new List<SettingInfo>();
                if (allSettingRelation == null || !allSettingRelation.Any())
                {
                    return subSettings;
                }

                foreach (var settingRelation in allSettingRelation)
                {
                    if (settingRelation.ParentId == parentSetting.Id)
                    {
                        for (var i = 0; i < allSetting.Count; i++)
                        {
                            var setting = allSetting[i];
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the number of feedback subject template.
        /// </summary>
        /// <returns>The number of feedback subject template.</returns>
        public int NumberOfFeedbackSubjectTemplate()
        {
            try
            {
                var feedbackTemplates = GetFeedbackSubjectTemplates();
                return feedbackTemplates.Count;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the feedback subject template by paging index.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template by paging index.</returns>
        public List<FeedbackTemplate> FeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            try
            {
                var result = new List<FeedbackTemplate>();
                int multipleNum = SettingConstants.MaxFeedbackSubjectTemplateNumberOnLoad;
                var feedbackTemplates = CacheService.Get<List<FeedbackTemplate>>(GetAllFeedbackTemplateSubjectSessionKey);

                if (feedbackTemplates == null)
                {
                    feedbackTemplates = GetFeedbackSubjectTemplates();
                }

                if (feedbackTemplates != null && feedbackTemplates.Count > 0)
                {
                    result = feedbackTemplates
                        .Skip((criteria.PagingIndex - 1) * multipleNum)
                        .Take(criteria.PagingIndex * multipleNum).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets all log actions.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The log actions list.</returns>
        public List<SettingInfo> AllLogAction(GetActionCriteria criteria)
        {
            try
            {
                List<SettingInfo> actions = null;
                string cachedKey = criteria.ActionType == SettingConstants.ActivityLog
                    ? GetAllActivityLogActionKey
                    : GetAllSettingLogActionKey;
                actions = CacheService.Get<List<SettingInfo>>(cachedKey);
                if (actions != null)
                {
                    return actions;
                }

                actions = _settingRepositoryInstance.GetAllLogAction(criteria);
                CacheService.Set(cachedKey, actions);
                return actions;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public List<SettingInfo> AllSiteType()
        {
            return new List<SettingInfo>()
            {
                new SettingInfo { Id = SiteConstants.All, Value = Labels.All },
                new SettingInfo { Id = SiteConstants.IBCGroup, Value = Labels.Credit },
                new SettingInfo { Id = SiteConstants.LicenseeGroup, Value = Labels.Licensee },
            };
        }

        /// <summary>
        /// Gets all inform headline settings.
        /// </summary>
        /// <returns>The Inform Headline Setting object.</returns>
        public InformHeadlineSetting AllInformHeadlineSettings()
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
                    Currency = GetAllCurrency(new GetAllCurrencyCriteria()),
                    Site = AllSite()[0].ChildSites.Select(s => new BaseModel { Id = s.Id, Name = s.Name }).ToList(),
                    ProductStatus = new List<BaseModel>() 
                    {
                        new BaseModel { Id = HeadlineProductStatusConstants.NotAvailable, Name = Labels.Symbol_NotApply },
                        new BaseModel { Id = HeadlineProductStatusConstants.Enable, Name = Labels.Enable },
                        new BaseModel { Id = HeadlineProductStatusConstants.Disable, Name = Labels.Disable }
                    },
                    Extensions = AllExtension()
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

        /// <summary>
        /// Gets the type of the feedback statistic filter.
        /// </summary>
        /// <returns>The Feedback statistic filter type list.</returns>
        public BaseModel[] FeedbackStatisticFilterTypeList()
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

        /// <summary>
        /// Resets all cache.
        /// </summary>
        public void ResetAllCache()
        {
            try
            {
                CacheService.Delete();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the main setting list.
        /// </summary>
        /// <returns>
        /// The main setting list.
        /// </returns>
        public List<SettingInfo> MainSetting()
        {
            try
            {
                var allSettings = AllSetting();
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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Get stations.
        /// </summary>
        /// <returns>List of stations.</returns>
        public List<Station> GetStations()
        {
            try
            {
                return _settingRepositoryInstance.GetStations(new GetStationsCriteria());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        private void GetSubSetting(string key, List<SettingInfo> settingList, IEnumerable<SettingInfo> allSetting)
        {
            var subList = allSetting.Where(s => s.SettingType == key).Select(s => s).OrderBy(s => s.DisplayOrder).ToList();
            var setting = new SettingInfo { SettingType = key, ChildSettings = subList };
            settingList.Add(setting);
        }

        /// <summary>
        /// Gets the setting relation by name.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting relation by name.</returns>
        private List<SettingInfo> GetSettingRelationByName(GetSettingRelationByNameCriteria criteria)
        {
            try
            {
                var settingRelations = new List<SettingInfo>();
                settingRelations = _settingRepositoryInstance.GetSettingRelationByName(criteria);
                if (settingRelations != null)
                {
                    settingRelations = settingRelations.OrderBy(setting => setting.DisplayOrder).ToList();
                }

                return settingRelations;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        private List<FeedbackTemplate> GetFeedbackSubjectTemplates()
        {
            try
            {
                var feedbackTemplates = _settingRepositoryInstance.GetFeedbackSubjectTemplateById(new GetFeedbackSubjectTemplateByIdCriteria
                {
                    TemplateId = 0
                });

                CacheService.Set(GetAllFeedbackTemplateSubjectSessionKey, feedbackTemplates);
                return feedbackTemplates;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}
