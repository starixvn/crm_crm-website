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
    using Fanex.CRM.Infrastructure.Helper;
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

        private readonly ISettingRepository _repositoryInstance;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new SettingRepository();
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
            var allSetting = new List<SettingInfo>();
            try
            {
                allSetting = CacheService.Get<List<SettingInfo>>(GetAllSettingSessionKey);
                if (allSetting == null)
                {
                    allSetting = _repositoryInstance.GetAllSetting(new GetAllSettingCriteria());
                }

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
            var result = new List<SiteInfo>();
            try
            {
                result = CacheService.Get<List<SiteInfo>>(GetAllSiteSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SiteInfo>();
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

                    result.Add(siteGroup);
                }

                CacheService.Set(GetAllSiteSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all title.
        /// </summary>
        /// <returns>All title.</returns>
        public List<SettingInfo> GetAllTitle()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllTitleSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SettingInfo>();
                result = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Title,
                    ParentId = 0
                });

                CacheService.Set(GetAllTitleSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <returns>All country.</returns>
        public List<CountryInfo> GetAllCountry()
        {
            var result = new List<CountryInfo>();
            try
            {
                result = CacheService.Get<List<CountryInfo>>(GetAllCountrySessionKey);
                if (result != null)
                {
                    return result;
                }

                result = _repositoryInstance.GetAllCountry(new GetAllCountryCriteria());
                CacheService.Set(GetAllCountrySessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all language.
        /// </summary>
        /// <returns>All language.</returns>
        public List<SettingInfo> GetAllLanguage()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllLanguageSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = _repositoryInstance.GetAllLanguage(new GetAllLanguageCriteria());
                CacheService.Set(GetAllLanguageSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all extension.
        /// </summary>
        /// <returns>All extension.</returns>
        public List<SettingInfo> GetAllExtension()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllExtensionSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SettingInfo>();
                result = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Extension,
                    ParentId = 0
                });

                CacheService.Set(GetAllExtensionSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> GetAllCommunicationTool()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllCommunicationToolSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = _repositoryInstance.GetAllCommunicationTool(new GetAllCommunicationToolCriteria());
                CacheService.Set(GetAllCommunicationToolSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllMainCategorySessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SettingInfo>();
                var rawData = _repositoryInstance.GetAllMainCategory(new GetAllMainCategoryCriteria());
                foreach (var data in rawData)
                {
                    data.ChildSettings = _repositoryInstance.GetAllSubCategory(new GetAllSubCategoryCriteria
                    {
                        ParentId = data.Id
                    });

                    data.ChildSettings = data.ChildSettings.OrderBy(s => s.DisplayOrder).ToList();
                    result.Add(data);
                }

                CacheService.Set(GetAllMainCategorySessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>
        public List<SettingInfo> GetAllProduct()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllProductSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SettingInfo>();
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Product,
                    ParentId = 0
                });

                var allSettingRelation = GetAllSettingRelation();
                var allSetting = GetAllSetting();

                var notApplySetting = new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.Symbol_NotApply
                };

                result.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.Symbol_NotApply,
                    ChildSettings = new List<SettingInfo> { notApplySetting }
                });

                foreach (var data in rawData)
                {
                    data.ChildSettings = GetSubSettingByParentId(data, allSettingRelation, allSetting);
                    data.ChildSettings.Insert(0, notApplySetting);
                    result.Add(data);
                }

                CacheService.Set(GetAllProductSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> GetAllStatus()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllStatusSessionKey);
                result = null;
                if (result != null)
                {
                    return result;
                }

                result = _repositoryInstance.GetAllStatus(new GetAllStatusCriteria());
                result = result.OrderBy(s => s.DisplayOrder).ToList();
                CacheService.Set(GetAllStatusSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all role.
        /// </summary>
        /// <returns>All role value.</returns>
        public List<SettingInfo> GetAllRole()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllRoleSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SettingInfo>();
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Role,
                    ParentId = 0
                });

                result = rawData;
                CacheService.Set(GetAllRoleSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets all response.
        /// </summary>
        /// <returns>All response.</returns>
        public List<SettingInfo> GetAllResponse()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = CacheService.Get<List<SettingInfo>>(GetAllResponseSessionKey);
                if (result != null)
                {
                    return result;
                }

                result = new List<SettingInfo>();
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Response,
                    ParentId = 0
                });

                result = rawData;
                CacheService.Set(GetAllResponseSessionKey, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
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
                listFeedbackTemplate = _repositoryInstance.GetFeedbackSubjectTemplateById(new GetFeedbackSubjectTemplateByIdCriteria
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
                    return Labels.IBC;
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
            var result = new List<SettingInfo>();
            try
            {
                result = _repositoryInstance.GetSettingRelationByName(criteria);
                if (result != null)
                {
                    result = result.OrderBy(setting => setting.DisplayOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
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
                settingRelations = _repositoryInstance.GetAllSettingRelation(new GetAllSettingRelationCriteria());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return settingRelations;
        }
    }
}
