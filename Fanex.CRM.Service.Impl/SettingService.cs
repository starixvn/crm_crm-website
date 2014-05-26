// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.DataAccess.Impl;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Service.Interface;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// Setting Service.
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly ISettingDataAccess _repository;
        private readonly IPermissionService _permissionService;

        public SettingService()
            : this(null, null)
        {
        }

        public SettingService(ISettingDataAccess repository, IPermissionService permissionService)
        {
            this._repository = repository ?? new SettingDataAccess();
            this._permissionService = permissionService ?? new PermissionService();
        }

        private IEnumerable<Setting> AllSettings
        {
            get { return GetAll(); }
        }

        private IEnumerable<string> AllSettingType
        {
            get
            {
                var allTypes = CacheService.Get<IEnumerable<string>>(Constants.AllSettingType);
                if (allTypes == null || allTypes.Count() <= 0)
                {
                    allTypes = AllSettings
                        .Select(s => s.Type.Trim())
                        .Distinct();

                    ////cache if collection has ele
                    CacheService.Set(Constants.AllSettingType, allTypes);
                }

                return allTypes;
            }
        }

        private IEnumerable<SettingRelation> AllSettingRelation
        {
            get { return GetAllSetingRelation(); }
        }

        /// <summary>
        /// Gets all settings.
        /// </summary>
        /// <returns>All settings.</returns>
        public IEnumerable<Setting> GetAll()
        {
            IEnumerable<Setting> settings = new List<Setting>();
            try
            {
                settings = CacheService.Get<IEnumerable<Setting>>(Constants.AllSettingKey);

                if (settings == null)
                {
                    settings = this._repository.FindAll();
                    if (settings != null && settings.Count() > 0)
                    {
                        CacheService.Set(Constants.AllSettingKey, settings);
                    }
                }
            }
            catch (Exception ex)
            {
                CacheService.Delete(Constants.AllSettingKey);
                settings = this._repository.FindAll();
                if (settings != null && settings.Count() > 0)
                {
                    CacheService.Set(Constants.AllSettingKey, settings);
                }

                Helper.SendMailErrorNotice(string.Format("(SettingService GetAll) - {0}", ex.Message));
                LogExtensions.ExceptionHandler(ex);
            }

            return settings;
        }

        /// <summary>
        /// Gets all setting and group by type.
        /// In order to access setting in group.
        /// </summary>
        /// <returns>Get result.</returns>
        public IEnumerable<Setting> GetAllGroupByType()
        {
            var settingGroupByType = new List<Setting>();
            try
            {
                var allSettings = AllSettings;
                var result = allSettings
                    .GroupBy(x => new { x.Type })
                    .Select(group => new { group.Key, Count = group.Count() });

                foreach (var s in result)
                {
                    GetSubSetting(s.Key.Type, ref settingGroupByType, allSettings);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetAllGroupByType) - {0}", ex.Message));
            }

            return settingGroupByType;
        }

        /// <summary>
        /// Creates the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Create result.</returns>
        public Result Create(Setting setting, string ip, int userId, int parentId)
        {
            Result result = new Result();
            try
            {
                Check.Argument.IsNotNull(setting, "setting");
                Check.Argument.IsValidIP(ip, "Ip");

                SettingType settingType = SettingType.BetType;

                if (!this._permissionService.HasPermission(FunctionPermission.SettingManagement, Constant.Policy.Host))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateSetting;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoPermission");
                    return result;
                }

                GetEnumValue<SettingType>(setting.Type, out settingType);

                setting.Id = GetNextIdByType(settingType);
                result = this._repository.Add(setting, ip, userId, parentId);

                ////clear cache
                if (result.ErrCode == 0)
                {
                    ////clear cache
                    if (result.ErrCode == 0)
                    {
                        ClearCache();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessage = string.Format(
                    "(SettingService Create) - Error: {0}<br> - userId: {1}<br> - SettingId: {2}<br> - Value: {3}<br> - Type: {4}",
                    ex.Message,
                    userId,
                    setting.Id,
                    setting.Value,
                    setting.Type);
                Helper.SendMailErrorNotice(errorMessage);
            }

            return result;
        }

        /// <summary>
        /// Updates the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Update result.</returns>
        public Result Update(Setting setting, string ip, int userId)
        {
            Check.Argument.IsNotNull(setting, "setting");
            Result result = new Result(0, string.Empty);
            try
            {
                if (!this._permissionService.HasPermission(FunctionPermission.SettingManagement.ToString(), Constant.Policy.Host))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateSetting;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoPermission");
                    return result;
                }

                if (IsExistedSetting(setting.Id))
                {
                    result = this._repository.Save(setting, ip, userId);
                    ////clear cache
                    if (result.ErrCode == 0)
                    {
                        ClearCache();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(
                    string.Format(
                        "(SettingService Update) - Error: {0}<br> - userId: {1}<br> - SettingId: {2}<br> - Value: {3}<br> - Type: {4}",
                        ex.Message,
                        userId,
                        setting.Id,
                        setting.Value,
                        setting.Type));
            }

            return result;
        }

        /// <summary>
        /// Deletes the specified setting identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Delete result.</returns>
        public Result Delete(int settingId, string ip, int userId)
        {
            Check.Argument.IsNotNegativeOrZero(settingId, "settingId");
            Result result = new Result(0, string.Empty);
            try
            {
                if (!this._permissionService.HasPermission(FunctionPermission.SettingManagement.ToString(), Constant.Policy.Host))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateSetting;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoPermission");
                    return result;
                }

                if (IsExistedSetting(settingId))
                {
                    result = this._repository.Remove(settingId, ip, userId);

                    ////clear cache
                    if (result.ErrCode == 0)
                    {
                        ClearCache();
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService Delete: {0}) - {1}<br> - Del by: {2}", settingId, ex.Message, userId));
            }

            return result;
        }

        /// <summary>
        /// Gets setting by identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <returns>Get result.</returns>
        public Setting GetById(int settingId)
        {
            var setting = new Setting() { Value = "N/A" };
            try
            {
                ////get from cache
                var settings = AllSettings;
                var query = settings.Where(s => s.Id == settingId).ToList();
                if (query != null && query.Count > 0)
                {
                    setting = query.First();
                }
                else
                {
                    setting = new Setting() { Value = "N/A" };
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetById: {0}) - {1}", ex.Message, settingId));
            }

            return setting;
        }

        /// <summary>
        /// Gets setting by type.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <returns>Get result.</returns>
        public IEnumerable<Setting> GetByType(string type)
        {
            try
            {
                string strType = type.ToString();
                var allSettings = AllSettings;
                var result = allSettings
                    .Where(s => s.Type == strType)
                    .Select(s => s).ToList();

                result.ForEach(s => s.Child = GetByParent(s.Id));
                return result.OrderBy(s => s.DisplayOrder);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetByType: {0}) - {1}", type, ex.Message));
                return new List<Setting>();
            }
        }

        /// <summary>
        /// Gets setting by type.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <param name="excludeItem">The exclude item.</param>
        /// <returns>Get result.</returns>
        public List<Setting> GetByType(string type, string excludeItem)
        {
            try
            {
                string strType = type.ToString();
                var allSettings = AllSettings;
                var result = allSettings
                    .Where(s => s.Type == strType && s.Value != excludeItem)
                    .Select(s => s).ToList();

                result.ForEach(s => s.Child = GetByParent(s.Id));

                return result.OrderBy(s => s.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessage = string.Format(
                    "(SettingService GetByType) - Error: {0}<br> - Type: {1}<br> - Exclude: {2}",
                    ex.Message,
                    type,
                    excludeItem);
                Helper.SendMailErrorNotice(errorMessage);
                return new List<Setting>();
            }
        }

        /// <summary>
        /// Gets the setting by type no child.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <returns>Get result.</returns>
        public IEnumerable<Setting> GetByTypeNoChild(string type)
        {
            try
            {
                string strType = type.ToString();
                var allSettings = AllSettings;
                var result = allSettings
                    .Where(s => s.Type == strType)
                    .Select(s => s).ToList();

                return result.OrderBy(s => s.DisplayOrder);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetByTypeNoChild: {0}) - {1}", type, ex.Message));
                return new List<Setting>();
            }
        }

        /// <summary>
        /// Gets setting by parent identify.
        /// Note: a setting can belongs to 2 other parent.
        /// </summary>
        /// <param name="parentId">Parent Identify.</param>
        /// <returns>Get result.</returns>
        public IEnumerable<Setting> GetByParent(int? parentId)
        {
            try
            {
                ////top settings have parentid = 0
                ////get list setting id by parentid
                var allSettingRelation = AllSettingRelation;
                var settingIds = allSettingRelation
                    .Where(r => parentId.HasValue
                        ? r.ParentId == parentId.Value
                        : r.ParentId == 0)
                     .Select(s => new { s.SettingId, s.Order });

                var allSettings = AllSettings;
                var query = allSettings.AsQueryable().Join(
                    settingIds,
                    setting => setting.Id,
                    parent => parent.SettingId,
                    (setting, parent) =>
                        new Setting
                        {
                            Id = setting.Id,
                            DisplayOrder = parent.Order,
                            Value = setting.Value,
                            Type = setting.Type
                        }).ToList().OrderBy(s => s.DisplayOrder);

                return query;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetByParent: {0}) - {1}", parentId, ex.Message));
                return new List<Setting>();
            }
        }

        /// <summary>
        /// Check existence of a specific setting.
        /// </summary>
        /// <param name="setting">Setting value.</param>
        /// <returns>Check result.</returns>
        public bool IsExistedSetting(Setting setting)
        {
            try
            {
                ////invalid input paremater 
                Check.Argument.IsNotNull(setting, "setting");
                ////Check.Argument.IsNotNegativeOrZero(setting.Id, "setting");

                var firstElement = AllSettings.Where(
                    s => (s.Value.Equals(setting.Value, StringComparison.OrdinalIgnoreCase)
                                && s.Type.Equals(setting.Type, StringComparison.OrdinalIgnoreCase)));

                return firstElement.Count() > 0;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessage = string.Format(
                    "(SettingService IsExistedSetting) - Error: {0} <br> - SettingId: {1}<br> - Value: {2}<br> - Type: {3}",
                    ex.Message,
                    setting.Id,
                    setting.Value,
                    setting.Type);
                Helper.SendMailErrorNotice(errorMessage);
                return false;
            }
        }

        /// <summary>
        /// Check a specific setting whether exist or not.
        /// </summary>
        /// <param name="settingId">Setting Identify.</param>
        /// <returns>Check result.</returns>
        public bool IsExistedSetting(int settingId)
        {
            try
            {
                Check.Argument.IsNotNegativeOrZero(settingId, "settingId");
                var firstElement = AllSettings.Where(s => s.Id == settingId);
                return firstElement.Count() > 0;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService IsExistedSetting: {0}) - {1}", settingId, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Search setting type support wild card.
        /// </summary>
        /// <param name="settingType">Setting Type.</param>
        /// <returns><![CDATA[IEnumerable<string>]]></returns>
        public IEnumerable<string> SearchSettingType(string settingType)
        {
            try
            {
                IEnumerable<string> settingSearchResult = new List<string>();

                if (!string.IsNullOrEmpty(settingType))
                {
                    var allSettings = AllSettingType;
                    settingSearchResult = allSettings
                        .Where(s => s.IsLike(settingType))
                        .Select(s => s);
                }

                return settingSearchResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService SearchSettingType: {0}) - {1}", settingType, ex.Message));
                return new List<string>();
            }
        }

        /// <summary>
        /// Gets the next identifier by type.
        /// </summary>
        /// <param name="type">The type value.</param>
        /// <returns>Get result.</returns>
        public int GetNextIdByType(SettingType type)
        {
            try
            {
                int lastSettingId = AllSettings
                    .OrderBy(setting => setting.Id)
                    .Last(setting => setting.Type == type.ToString())
                    .Id;

                ////get the next settting id
                lastSettingId++;

                ////check the next setting id whether exist or not
                var existintionSettings = AllSettings.Where(setting => setting.Id == lastSettingId);

                ////if the next setting is also already exist, select the lasted next from all setting
                if (existintionSettings.Count() > 0)
                {
                    int maxId = AllSettings.OrderByDescending(s => s.Id).First().Id;
                    return maxId++;
                }

                return lastSettingId;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetNextIdByType) - {0}", ex.Message));
                return 0;
            }
        }

        /// <summary>
        /// Gets the setting relation by name.
        /// </summary>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Get result.</returns>
        public IEnumerable<Setting> GetSettingRelationByName(string settingName, int parentId)
        {
            try
            {
                return this._repository.GetSettingRelationByName(settingName, parentId)
                    .Select(s => s);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessage = string.Format(
                    "(SettingService GetSettingRelationByName) - Error: {0}<br> - Name: {1}<br> - parentId: {2}",
                    ex.Message,
                    settingName,
                    parentId);
                Helper.SendMailErrorNotice(errorMessage);
                return new List<Setting>();
            }
        }

        /// <summary>
        /// Creates the specified setting identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Create result.</returns>
        public Result Create(string settingId, string settingName, string ip, int userId, int parentId)
        {
            Check.Argument.IsNotNull(settingId, "settingId");
            Check.Argument.IsNotNull(parentId, "parentId");
            Check.Argument.IsValidIP(ip, "Ip");
            Result result = new Result(0, string.Empty);
            try
            {
                if (!this._permissionService.HasPermission(FunctionPermission.SettingRelationManagement.ToString(), Constant.Policy.Host))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateSetting;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoPermission");
                    return result;
                }

                result = this._repository.Add(settingId, settingName, ip, userId, parentId);
                if (result.IsSuccessfull)
                {
                    ClearCache();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessage = string.Format(
                        "(SettingService Create) - Error: {0}<br> - SettignId: {1}<br> - SettignName: {2}<br> - Parent: {3}<br> - User: {4}",
                        ex.Message,
                        settingId,
                        settingName,
                        parentId,
                        userId);
                Helper.SendMailErrorNotice(errorMessage);
            }

            return result;
        }

        /// <summary>
        /// Deletes the specified setting identifier.
        /// </summary>
        /// <param name="settingId">The setting identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Delete result.</returns>
        public Result Delete(int settingId, string ip, int userId, int parentId)
        {
            Check.Argument.IsNotNull(settingId, "settingId");
            Check.Argument.IsNotNull(parentId, "parentId");
            Check.Argument.IsValidIP(ip, "Ip");
            Result result = new Result(0, string.Empty);
            try
            {
                if (!this._permissionService.HasPermission(FunctionPermission.SettingRelationManagement.ToString(), Constant.Policy.Host))
                {
                    result.ErrCode = (int)Enumerations.ErrorCode.UpdateSetting;
                    result.ErrMsg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "lblNoPermission");
                    return result;
                }

                result = this._repository.Remove(settingId, ip, userId, parentId);
                if (result.IsSuccessfull)
                {
                    ClearCache();
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                var errorMessage = string.Format(
                        "(SettingService Delete) - Error: {0}<br> - SettignId: {1}<br> - Parent: {2}<br> - User: {3}",
                        ex.Message,
                        settingId,
                        parentId,
                        userId);
                Helper.SendMailErrorNotice(errorMessage);
            }

            return result;
        }

        #region SelAllSetting

        /// <summary>
        /// Selects all settings.
        /// </summary>
        /// <returns>All settings.</returns>
        public AllSetting SelAllSetting()
        {
            AllSetting allSetting = new AllSetting();
            try
            {
                allSetting.Title = new List<Setting>();
                allSetting.Title = GetByType("Title").ToList();

                allSetting.Category = new List<Setting>();
                allSetting.Category = GetByType("MainCategory").ToList();

                allSetting.Product = new List<Setting>();
                allSetting.Product = GetByType("Product").ToList();

                allSetting.Role = new List<Setting>();
                allSetting.Role = GetByType("Role").ToList();

                /*allSetting.Status = new List<Setting>();
                allSetting.Status = GetByType("Status").ToList();*/
                allSetting.Status = GetStatusSetting(1);

                allSetting.Response = new List<Setting>();
                allSetting.Response = GetByType("Response").ToList();

                allSetting.Communication = new List<Setting>();
                allSetting.Communication = GetByType("Communication").ToList();

                allSetting.Language = new List<Setting>();
                allSetting.Language = GetByType("Language").ToList();

                allSetting.Extension = new List<Setting>();
                allSetting.Extension = GetByType("Station").ToList();

                ICountryService countryService = new CountryService();
                allSetting.Country = new List<Country>();
                allSetting.Country = countryService.GetAll().ToList();

                allSetting.Site = new List<Site>();
                allSetting.Site = SelAllSite();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allSetting;
        }
        #endregion

        #region SelAllSite

        /// <summary>
        /// Selects all sites.
        /// </summary>
        /// <returns>All sites.</returns>
        public List<Site> SelAllSite()
        {
            var result = new List<Site>();
            try
            {
                var crmSite = _repository.GetSiteSetting();
                foreach (SiteSetting site in crmSite)
                {
                    var newSite = new Site()
                    {
                        SiteName = site.Value,
                        SiteId = site.Id.ToString(),
                        Order = site.DisplayOrder,
                        SiteType = site.IsLicensee ? 1 : 0
                    };

                    result.Add(newSite);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService SelAllSite) - {0}", ex.Message));
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Gets the status setting.
        /// </summary>
        /// <param name="type">The type parameter.</param>
        /// <returns>The setting list.</returns>
        public IList<Setting> GetStatusSetting(int type)
        {
            IList<Setting> settings = null;
            string key = type == 1 ? Constants.FeedbackStatusKey : Constants.AllStatusKey;
            try
            {
                settings = CacheService.Get<IList<Setting>>(key);
                if (settings != null)
                {
                    return settings;
                }

                settings = this._repository.GetStatusSetting(type);
                if (settings != null)
                {
                    CacheService.Set(key, settings);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                Helper.SendMailErrorNotice(string.Format("(SettingService GetStatusSetting) - {0}", ex.Message));
            }

            return settings;
        }

        private static void ClearCache()
        {
            CacheService.Delete(Constants.AllSettingKey);
            CacheService.Delete(Constants.SettingRelationKey);
        }

        private void GetSubSetting(string key, ref List<Setting> settingList, IEnumerable<Setting> allSetting)
        {
            var subList = allSetting
                .Where(s => s.Type == key)
                .Select(s => s).ToList();

            var setting = new Setting { Type = key, Child = subList };
            settingList.Add(setting);
        }

        /// <summary>
        /// Gets all setting relation.
        /// </summary>
        /// <returns>All setting relation.</returns>
        private IEnumerable<SettingRelation> GetAllSetingRelation()
        {
            IEnumerable<SettingRelation> settingRelations = new List<SettingRelation>();
            try
            {
                settingRelations =
                CacheService.Get<IEnumerable<SettingRelation>>(Constants.SettingRelationKey);
                ////perfrom access repository to get data
                if (settingRelations == null || settingRelations.Count() <= 0)
                {
                    settingRelations = _repository.GetSettingRelation();
                    ////cache seting relation
                    if (settingRelations != null && settingRelations.Count() > 0)
                    {
                        CacheService.Set(Constants.SettingRelationKey, settingRelations);
                    }
                }
            }
            catch (Exception ex)
            {
                CacheService.Delete(Constants.SettingRelationKey);
                settingRelations = this._repository.GetSettingRelation();
                if (settingRelations != null && settingRelations.Count() > 0)
                {
                    CacheService.Set(Constants.SettingRelationKey, settingRelations);
                }

                LogExtensions.ExceptionHandler(ex);
            }

            return settingRelations;
        }

        /// <summary>
        /// Gets the enumeration value.
        /// </summary>
        /// <typeparam name="T">Type value.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="returnValue">The return value.</param>
        /// <returns>The enumeration value.</returns>
        private bool GetEnumValue<T>(string input, out T returnValue)
             where T : struct, IComparable, IFormattable, IConvertible
        {
            if (Enum.IsDefined(typeof(T), input))
            {
                returnValue = (T)Enum.Parse(typeof(T), input, true);
                return true;
            }
            else
            {
                ////input not found in the Enum, fill the out parameter
                ////  with the first item from the enum
                string[] values = Enum.GetNames(typeof(T));
                returnValue = (T)Enum.Parse(typeof(T), values[0], true);
                return false;
            }
        }
    }
}
