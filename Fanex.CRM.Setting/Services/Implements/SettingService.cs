// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories.Implements;
    using Fanex.CRM.Setting.Repositories.Interfaces;
    using Fanex.CRM.Setting.Resources;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Setting Service.
    /// </summary>
    public class SettingService : ISettingService
    {
        public const string GetAllSettingSessionKey = "Fanex.CRM.Setting.GetAllSettingSessionKey";
        public const string GetAllCountrySessionKey = "Fanex.CRM.Setting.GetAllCountrySessionKey";
        public const string GetAllTitleSessionKey = "Fanex.CRM.Setting.GetAllTitleSessionKey";
        public const string GetAllFeedbackTemplateSubjectSessionKey = "Fanex.CRM.Setting.GetAllFeedbackTemplateSubjectSessionKey";

        public const string GetAllSiteSessionKey = "Fanex.CRM.Setting.GetAllSiteSessionKey";
        public const string GetAllLanguageSessionKey = "Fanex.CRM.Setting.GetAllLanguageSessionKey";
        public const string GetAllExtensionSessionKey = "Fanex.CRM.Setting.GetAllExtensionSessionKey";
        public const string GetAllCommunicationToolSessionKey = "Fanex.CRM.Setting.GetAllCommunicationToolSessionKey";
        public const string GetAllMainCategorySessionKey = "Fanex.CRM.Setting.GetAllMainCategorySessionKey";
        public const string GetAllProductSessionKey = "Fanex.CRM.Setting.GetAllProductSessionKey";
        public const string GetAllStatusSessionKey = "Fanex.CRM.Setting.GetAllStatusSessionKey";
        public const string GetAllRoleSessionKey = "Fanex.CRM.Setting.GetAllRoleSessionKey";
        public const string GetAllResponseSessionKey = "Fanex.CRM.Setting.GetAllResponseSessionKey";
        public const string GetAllActivityLogActionKey = "Fanex.CRM.Setting.GetAllActivityLogActionKey";
        public const string GetAllSettingLogActionKey = "Fanex.CRM.Setting.GetAllSettingLogActionKey";

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
            SettingInfo setting = null;
            try
            {
                var settings = GetAllSetting();
                setting = settings.Where(s => s.Id == criteria.SettingId).FirstOrDefault();
                if (setting == null)
                {
                    setting = new SettingInfo()
                    {
                        Value = Labels.Symbol_NotApply
                    };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return setting;
        }

        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <returns>All setting.</returns>
        public List<SettingInfo> GetAllSetting()
        {
            List<SettingInfo> allSetting = null;
            try
            {
                allSetting = CacheService.Get<List<SettingInfo>>(GetAllSettingSessionKey);
                if (allSetting != null)
                {
                    return allSetting;
                }

                allSetting = _settingRepositoryInstance.GetAllSetting(new GetAllSettingCriteria());
                CacheService.Set(GetAllSettingSessionKey, allSetting);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allSetting;
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <returns>All site value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<SiteInfo> GetAllSite()
        {
            var allSite = new List<SiteInfo>();
            try
            {
                allSite = CacheService.Get<List<SiteInfo>>(GetAllSiteSessionKey);
                if (allSite != null)
                {
                    return allSite;
                }

                allSite = new List<SiteInfo>();
                var crmSite = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Site,
                    ParentId = 0
                });

                var rawDataGroups = crmSite.GroupBy(setting => GetSiteGroupId(setting.Id));
                foreach (var rawDataGroup in rawDataGroups)
                {
                    var siteGroup = new SiteInfo
                    {
                        Id = rawDataGroup.Key,
                        ChildSites = new List<SiteInfo>()
                    };

                    siteGroup.Name = GetSiteGroupName(siteGroup.Id);
                    foreach (var rawDataSetting in rawDataGroup)
                    {
                        var newSite = new SiteInfo()
                        {
                            Name = rawDataSetting.Value,
                            Id = rawDataSetting.Id
                        };

                        siteGroup.ChildSites.Add(newSite);
                    }

                    allSite.Add(siteGroup);
                }

                CacheService.Set(GetAllSiteSessionKey, allSite);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allSite;
        }

        /// <summary>
        /// Gets all title.
        /// </summary>
        /// <returns>All title.</returns>
        public List<SettingInfo> GetAllTitle()
        {
            var allTitle = new List<SettingInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allTitle;
        }

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <returns>All country.</returns>
        public List<CountryInfo> GetAllCountry()
        {
            var allCountry = new List<CountryInfo>();
            try
            {
                allCountry = CacheService.Get<List<CountryInfo>>(GetAllCountrySessionKey);
                if (allCountry != null)
                {
                    return allCountry;
                }

                allCountry = _settingRepositoryInstance.GetAllCountry(new GetAllCountryCriteria());
                CacheService.Set(GetAllCountrySessionKey, allCountry);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCountry;
        }

        /// <summary>
        /// Gets all language.
        /// </summary>
        /// <returns>All language.</returns>
        public List<SettingInfo> GetAllLanguage()
        {
            var allLanguage = new List<SettingInfo>();
            try
            {
                allLanguage = CacheService.Get<List<SettingInfo>>(GetAllLanguageSessionKey);
                if (allLanguage != null)
                {
                    return allLanguage;
                }

                allLanguage = _settingRepositoryInstance.GetAllLanguage(new GetAllLanguageCriteria());
                CacheService.Set(GetAllLanguageSessionKey, allLanguage);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allLanguage;
        }

        /// <summary>
        /// Gets all extension.
        /// </summary>
        /// <returns>All extension.</returns>
        public List<SettingInfo> GetAllExtension()
        {
            var allExtension = new List<SettingInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allExtension;
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> GetAllCommunicationTool()
        {
            var allCommunicationTool = new List<SettingInfo>();
            try
            {
                allCommunicationTool = CacheService.Get<List<SettingInfo>>(GetAllCommunicationToolSessionKey);
                if (allCommunicationTool != null)
                {
                    return allCommunicationTool;
                }

                allCommunicationTool = _settingRepositoryInstance.GetAllCommunicationTool(new GetAllCommunicationToolCriteria());
                allCommunicationTool = allCommunicationTool.OrderBy(s => s.DisplayOrder).ToList();
                CacheService.Set(GetAllCommunicationToolSessionKey, allCommunicationTool);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCommunicationTool;
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory()
        {
            var allMainCategory = new List<SettingInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allMainCategory;
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public List<SettingInfo> GetAllProduct()
        {
            var allProduct = new List<SettingInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allProduct;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// All status.
        /// </returns>
        public List<SettingInfo> GetAllStatus(GetAllStatusCriteria criteria)
        {
            var allStatus = new List<SettingInfo>();
            try
            {
                allStatus = CacheService.Get<List<SettingInfo>>(GetAllStatusSessionKey);
                allStatus = null;
                if (allStatus != null)
                {
                    return allStatus;
                }

                allStatus = _settingRepositoryInstance.GetAllStatus(criteria);
                allStatus = allStatus.OrderBy(s => s.DisplayOrder).ToList();
                CacheService.Set(GetAllStatusSessionKey, allStatus);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allStatus;
        }

        /// <summary>
        /// Gets all role.
        /// </summary>
        /// <returns>All role value.</returns>
        public List<SettingInfo> GetAllRole()
        {
            var allRole = new List<SettingInfo>();
            try
            {
                allRole = CacheService.Get<List<SettingInfo>>(GetAllRoleSessionKey);
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allRole;
        }

        /// <summary>
        /// Gets all response.
        /// </summary>
        /// <returns>All response.</returns>
        public List<SettingInfo> GetAllResponse()
        {
            var allResponse = new List<SettingInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allResponse;
        }

        /// <summary>
        /// Gets all customer type.
        /// </summary>
        /// <returns>All customer type.</returns>
        public List<SettingInfo> GetAllCustomerType()
        {
            var allCustomerType = new List<SettingInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCustomerType;
        }

        /// <summary>
        /// Gets all VIP type.
        /// </summary>
        /// <returns>All VIP type.</returns>
        public List<SettingInfo> GetAllVIPType()
        {
            var allVIPType = new List<SettingInfo>();
            try
            {
                allVIPType.Add(new SettingInfo
                {
                    Id = VIPTypeConstants.All,
                    Value = Labels.All
                });

                allVIPType.Add(new SettingInfo
                {
                    Id = VIPTypeConstants.VIP,
                    Value = Labels.VIP
                });

                allVIPType.Add(new SettingInfo
                {
                    Id = VIPTypeConstants.None,
                    Value = Labels.None
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allVIPType;
        }

        /// <summary>
        /// Gets the sub setting by parent identifier.
        /// </summary>
        /// <param name="parentSetting">The parent setting.</param>
        /// <param name="allSettingRelation">All setting relation.</param>
        /// <param name="allSetting">All setting.</param>
        /// <returns>The sub setting by parent identifier.</returns>
        public List<SettingInfo> GetSubSettingByParentId(
            SettingInfo parentSetting,
            List<SettingInfo> allSettingRelation,
            List<SettingInfo> allSetting)
        {
            List<SettingInfo> subSettings = new List<SettingInfo>();
            try
            {
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
                                    Type = setting.Type,
                                    ParentId = parentSetting.Id,
                                    IsSubSetting = true
                                });
                            }
                        }
                    }
                }

                subSettings = subSettings.OrderBy(s => s.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return subSettings;
        }

        /// <summary>
        /// Gets the number of feedback subject template.
        /// </summary>
        /// <returns>The number of feedback subject template.</returns>
        public int GetNumberOfFeedbackSubjectTemplate()
        {
            List<FeedbackTemplate> listFeedbackTemplate = new List<FeedbackTemplate>();
            try
            {
                listFeedbackTemplate = _settingRepositoryInstance.GetFeedbackSubjectTemplateById(new GetFeedbackSubjectTemplateByIdCriteria
                {
                    TemplateId = 0
                });

                CacheService.Set(GetAllFeedbackTemplateSubjectSessionKey, listFeedbackTemplate);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return listFeedbackTemplate.Count;
        }

        /// <summary>
        /// Gets the feedback subject template by paging index.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template by paging index.</returns>
        public List<FeedbackTemplate> GetFeedbackSubjectTemplateByPagingIndex(GetFeedbackSubjectTemplateByPagingIndexCriteria criteria)
        {
            var result = new List<FeedbackTemplate>();
            try
            {
                int multipleNum = SettingConstants.MaxFeedbackSubjectTemplateNumberOnLoad;
                List<FeedbackTemplate> listFeedbackTemplate = CacheService.Get<List<FeedbackTemplate>>(GetAllFeedbackTemplateSubjectSessionKey);
                if (listFeedbackTemplate != null && listFeedbackTemplate.Count > 0)
                {
                    result = listFeedbackTemplate
                        .Skip((criteria.PagingIndex - 1) * multipleNum)
                        .Take(criteria.PagingIndex * multipleNum).ToList();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all log actions.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The log actions list.</returns>
        public List<SettingInfo> GetAllLogAction(GetActionCriteria criteria)
        {
            List<SettingInfo> actions = null;
            string cachedKey = criteria.Type == SettingConstants.ActivityLog
                ? GetAllActivityLogActionKey
                : GetAllSettingLogActionKey;
            try
            {
                actions = CacheService.Get<List<SettingInfo>>(cachedKey);
                if (actions != null)
                {
                    return actions;
                }

                actions = _settingRepositoryInstance.GetAllLogAction(criteria);
                CacheService.Set(cachedKey, actions);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return actions;
        }

        /// <summary>
        /// Resets all cache.
        /// </summary>
        public void ResetAllCache()
        {
            try
            {
                CacheService.Delete(GetAllSettingSessionKey);
                CacheService.Delete(GetAllCountrySessionKey);
                CacheService.Delete(GetAllTitleSessionKey);
                CacheService.Delete(GetAllFeedbackTemplateSubjectSessionKey);

                CacheService.Delete(GetAllSiteSessionKey);
                CacheService.Delete(GetAllLanguageSessionKey);
                CacheService.Delete(GetAllExtensionSessionKey);
                CacheService.Delete(GetAllCommunicationToolSessionKey);
                CacheService.Delete(GetAllMainCategorySessionKey);
                CacheService.Delete(GetAllProductSessionKey);
                CacheService.Delete(GetAllStatusSessionKey);
                CacheService.Delete(GetAllRoleSessionKey);
                CacheService.Delete(GetAllResponseSessionKey);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private int GetSiteGroupId(int siteId)
        {
            return IsLicenseeSite(siteId) ? SiteConstants.LicenseeGroup : SiteConstants.IBCGroup;
        }

        private bool IsLicenseeSite(int siteId)
        {
            return siteId >= SiteConstants.BeginLicenseeSiteId;
        }

        /// <summary>
        /// Gets the name of the site group.
        /// </summary>
        /// <param name="siteGroupId">The site group identifier.</param>
        /// <returns>The name of the site group.</returns>
        private string GetSiteGroupName(int siteGroupId)
        {
            switch (siteGroupId)
            {
                case SiteConstants.LicenseeGroup:
                    return Labels.Licensee;
                case SiteConstants.IBCGroup:
                    return Labels.Credit;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the setting relation by name.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting relation by name.</returns>
        private List<SettingInfo> GetSettingRelationByName(GetSettingRelationByNameCriteria criteria)
        {
            var settingRelations = new List<SettingInfo>();
            try
            {
                settingRelations = _settingRepositoryInstance.GetSettingRelationByName(criteria);
                if (settingRelations != null)
                {
                    settingRelations = settingRelations.OrderBy(setting => setting.DisplayOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return settingRelations;
        }

        /// <summary>
        /// Gets all setting relation.
        /// </summary>
        /// <returns>All setting relation.</returns>
        private List<SettingInfo> GetAllSettingRelation()
        {
            List<SettingInfo> settingRelations = new List<SettingInfo>();
            try
            {
                settingRelations = _settingRepositoryInstance.GetAllSettingRelation(new GetAllSettingRelationCriteria());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return settingRelations;
        }
    }
}
