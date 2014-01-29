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
    using System.Linq;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
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
        public const string GetAllSettingRelationSessionKey = "Fanex.CRM.Setting.GetAllSettingRelationSessionKey";
        public const string GetAllFeedbackTemplateSubjectSessionKey = "Fanex.CRM.Setting.GetAllFeedbackTemplateSubjectSessionKey";

        private readonly ISettingRepository _repositoryInstance;

        public SettingService()
            : this(null)
        {
        }

        public SettingService(ISettingRepository repositoryInstance)
        {
            _repositoryInstance = repositoryInstance ?? new SettingRepository();
        }

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
                Helper.SendMailErrorNotice(string.Format(Labels.Error_GetSetting, ex.Message, criteria.SettingId));
            }

            return setting;
        }

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
                Helper.SendMailErrorNotice(string.Format(Labels.Error_GetAllSetting, ex.Message));
            }

            return allSetting;
        }

        public List<SiteInfo> GetAllSite()
        {
            var result = new List<SiteInfo>();
            try
            {
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format(Labels.Error_GetAllSite, ex.Message));
            }

            return result;
        }

        public List<CountryInfo> GetAllCountry()
        {
            var result = new List<CountryInfo>();
            try
            {
                result = CacheService.Get<List<CountryInfo>>(GetAllCountrySessionKey);
                if (result == null)
                {
                    result = _repositoryInstance.GetAllCountry(new GetAllCountryCriteria());
                    if (result != null && result.Count > 0)
                    {
                        CacheService.Set(GetAllCountrySessionKey, result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                CacheService.Delete(GetAllCountrySessionKey);
            }

            return result;
        }

        public List<SettingInfo> GetAllLanguage()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Language,
                    ParentId = 0
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllExtension()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Extension,
                    ParentId = 0
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllCommunicationTool()
        {
            var result = new List<SettingInfo>();
            try
            {
                result = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.CommunicationTool,
                    ParentId = 0
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllMainCategory()
        {
            var result = new List<SettingInfo>();
            try
            {
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                 {
                     SettingName = SettingTypeConstants.MainCategory,
                     ParentId = 0
                 });

                var allSettingRelation = GetAllSetingRelation();
                var allSetting = GetAllSetting();
                foreach (var data in rawData)
                {
                    data.ChildSettings = GetSubSettingByParentId(data, allSettingRelation, allSetting);
                    result.Add(data);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllProduct()
        {
            var result = new List<SettingInfo>();
            try
            {
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Product,
                    ParentId = 0
                });

                var allSettingRelation = GetAllSetingRelation();
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
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllStatus()
        {
            var result = new List<SettingInfo>();
            try
            {
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Status,
                    ParentId = 0
                });

                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllRole()
        {
            var result = new List<SettingInfo>();
            try
            {
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Role,
                    ParentId = 0
                });

                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<SettingInfo> GetAllResponse()
        {
            var result = new List<SettingInfo>();
            try
            {
                var rawData = GetSettingRelationByName(new GetSettingRelationByNameCriteria
                {
                    SettingName = SettingTypeConstants.Response,
                    ParentId = 0
                });

                result = rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

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
                        foreach (var setting in allSetting)
                        {
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

        private int GetSiteGroupId(int siteId)
        {
            return IsLicenseeSite(siteId) ? SiteConstants.LicenseeGroup : SiteConstants.IBCGroup;
        }

        private bool IsLicenseeSite(int siteId)
        {
            return siteId >= SiteConstants.BeginLicenseeSiteId;
        }

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
                var errorMessage = string.Format(
                       Labels.Error_GetSettingRelationByName,
                       ex.Message,
                       criteria.SettingName,
                       criteria.ParentId);
                Helper.SendMailErrorNotice(errorMessage);
            }

            return result;
        }

        private List<SettingInfo> GetAllSetingRelation()
        {
            List<SettingInfo> settingRelations = new List<SettingInfo>();
            try
            {
                settingRelations = CacheService.Get<List<SettingInfo>>(GetAllSettingRelationSessionKey);
                if (settingRelations == null || !settingRelations.Any())
                {
                    settingRelations = _repositoryInstance.GetAllSetingRelation(new GetAllSettingRelationCriteria());
                }

                if (settingRelations != null && settingRelations.Any())
                {
                    CacheService.Set(GetAllSettingRelationSessionKey, settingRelations);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                CacheService.Delete(GetAllSettingRelationSessionKey);
            }

            return settingRelations;
        }
    }
}
