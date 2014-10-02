// --------------------------------------------------------------------------
// <copyright file="SettingManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.AdminTool.Criteria.SettingManagement;
    using Fanex.CRM.AdminTool.Repositories.SettingManagement;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Infrastructure.Service.Implements;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Setting Relation Service.
    /// </summary>
    public class SettingManagementService : ISettingManagementService
    {
        private const string AllSettingSessionKey = "Fanex.CRM.AdminTool.Services.SettingRelation.GetAllSettingSessionKey";

        private ISettingManagementRepository _settingManagementRepository;

        private ISettingService _settingService;

        public SettingManagementService()
            : this(null, null)
        {
        }

        public SettingManagementService(
            ISettingManagementRepository settingManagementRepository,
            ISettingService settingService)
        {
            _settingManagementRepository = settingManagementRepository ?? new SettingManagementRepository();
            _settingService = settingService ?? new SettingService();
        }

        /// <summary>
        /// Gets the main setting list.
        /// </summary>
        /// <returns>
        /// The main setting list.
        /// </returns>
        public List<SettingInfo> GetMainSetting()
        {
            try
            {
                var allSettings = AllSetting();
                var settingGroups = allSettings
                    .GroupBy(x => new { x.Type })
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
        /// Gets the parent setting list.
        /// </summary>
        /// <returns>
        /// The parent setting list.
        /// </returns>
        public List<SettingInfo> GetParentSetting()
        {
            try
            {
                var allSettings = AllSetting();
                return allSettings.OrderBy(s => s.Type).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertSetting(InsertSettingCriteria criteria)
        {
            try
            {
                Result result = IsExistedSetting(criteria.NewSetting) ?
                    new Result(ErrorCodeConstants.ServerError, Labels.SettingManagement_Error_ExistedSetting) :
                    _settingManagementRepository.InsertSetting(criteria);

                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.SettingManagement_InsertSetting_InsertSuccess;
                    _settingService.ResetAllCache();
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
        /// Gets the setting by type.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting by type.</returns>
        public List<SettingInfo> SettingByType(GetSettingByTypeCriteria criteria)
        {
            try
            {
                var allSettings = AllSetting();
                var result = allSettings
                    .Where(s => s.Type == criteria.MainSetting.Type)
                    .Select(s => s)
                    .ToList();
                return result.OrderBy(s => s.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Check existence of a specific setting.
        /// </summary>
        /// <param name="setting">Setting value.</param>
        /// <returns>
        /// Check result.
        /// </returns>
        public bool IsExistedSetting(SettingInfo setting)
        {
            if (setting == null)
            {
                return false;
            }

            var firstElement = AllSetting().Where(
                s => string.Equals(s.Value, setting.Value, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(s.Type, setting.Type, StringComparison.OrdinalIgnoreCase));
            return firstElement.Any();
        }

        /// <summary>
        /// Deletes the setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Delete result.</returns>
        public Result DeleteSetting(DeleteSettingCriteria criteria)
        {
            try
            {
                Result result = new Result();
                if (IsExistedSetting(criteria.OldSetting))
                {
                    result = _settingManagementRepository.DeleteSetting(criteria);
                }

                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.SettingManagement_DeleteSettingSuccess;
                    _settingService.ResetAllCache();
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
        /// Edits the setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Edit result.</returns>
        public Result EditSetting(EditSettingCriteria criteria)
        {
            try
            {
                Result result = !string.Equals(criteria.OldSetting.Value, criteria.UpdatedSetting.Value, StringComparison.OrdinalIgnoreCase) &&
                    IsExistedSetting(criteria.UpdatedSetting) ?
                    new Result(ErrorCodeConstants.ServerError, Labels.SettingManagement_Error_ExistedSetting) :
                    _settingManagementRepository.EditSetting(criteria);

                if (result != null && result.Code == ErrorCodeConstants.NoneError)
                {
                    result.Message = Labels.SettingManagement_EditSetting_EditSuccess;
                    _settingService.ResetAllCache();
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
        /// Gets the setting by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting by identifier.</returns>
        public SettingInfo GetSettingById(GetSettingByIdCriteria criteria)
        {
            try
            {
                return AllSetting()
                    .Where(s => s.Id == criteria.SettingId)
                    .ToList()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        private List<SettingInfo> AllSetting()
        {
            var allSetting = CacheService.Get<List<SettingInfo>>(AllSettingSessionKey);
            if (allSetting == null)
            {
                allSetting = _settingManagementRepository.AllSetting(new GetAllSettingCriteria());
                CacheService.Set(AllSettingSessionKey, allSetting);
            }

            return allSetting;
        }

        private void GetSubSetting(string key, List<SettingInfo> settingList, IEnumerable<SettingInfo> allSetting)
        {
            var subList = allSetting.Where(s => s.Type == key).Select(s => s).OrderBy(s => s.DisplayOrder).ToList();
            var setting = new SettingInfo { Type = key, ChildSettings = subList };
            settingList.Add(setting);
        }
    }
}
