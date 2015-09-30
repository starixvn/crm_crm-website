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
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Helpers;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services;

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

        public IEnumerable<SettingInfo> MainSetting
        {
            get
            {
                return _settingService.GetMainSettings();
            }
        }

        /// <summary>
        /// Gets the parent setting.
        /// </summary>
        /// <value>
        /// The parent setting.
        /// </value>
        public IEnumerable<SettingInfo> ParentSetting
        {
            get
            {
                var allSettings = _settingService.GetAllSettings();
                return allSettings.OrderBy(s => s.SettingType).ToList();
            }
        }

        /// <summary>
        /// Inserts setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertSetting(InsertSettingCriteria criteria)
        {
            Result result = IsExistedSetting(criteria.NewSetting) ?
                new Result(ErrorCodeConstants.ServerError, Labels.SettingManagement_Error_ExistedSetting) :
                _settingManagementRepository.InsertSetting(criteria);

            if (result != null && result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.SettingManagement_InsertSetting_InsertSuccess;
                _settingService.ClearCache();
            }

            return result;
        }

        public IEnumerable<SettingInfo> SettingByType(GetSettingByTypeCriteria criteria)
        {
            var allSettings = _settingService.GetAllSettings();
            var result = allSettings
                .Where(s => s.SettingType == criteria.MainSetting.SettingType)
                .Select(s => s)
                .ToList();
            return result.OrderBy(s => s.DisplayOrder).ToList();
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

            var firstElement = _settingService.GetAllSettings().Where(
                s => string.Equals(s.Value, setting.Value, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(s.SettingType, setting.SettingType, StringComparison.OrdinalIgnoreCase));
            return firstElement.Any();
        }

        /// <summary>
        /// Deletes the setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Delete result.</returns>
        public Result DeleteSetting(DeleteSettingCriteria criteria)
        {
            Result result = new Result();
            if (IsExistedSetting(criteria.OldSetting))
            {
                result = _settingManagementRepository.DeleteSetting(criteria);
            }

            if (result != null && result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.SettingManagement_DeleteSettingSuccess;
                _settingService.ClearCache();
            }

            return result;
        }

        /// <summary>
        /// Edits the setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Edit result.</returns>
        public Result EditSetting(EditSettingCriteria criteria)
        {
            Result result = !string.Equals(criteria.OldSetting.Value, criteria.UpdatedSetting.Value, StringComparison.OrdinalIgnoreCase) &&
                IsExistedSetting(criteria.UpdatedSetting) ?
                new Result(ErrorCodeConstants.ServerError, Labels.SettingManagement_Error_ExistedSetting) :
                _settingManagementRepository.EditSetting(criteria);

            if (result != null && result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.SettingManagement_EditSetting_EditSuccess;
                _settingService.ClearCache();
            }

            return result;
        }

        /// <summary>
        /// Gets the setting by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting by identifier.</returns>
        public SettingInfo GetSettingById(GetSettingByIdCriteria criteria)
        {
            return _settingService.GetAllSettings()
                .Where(s => s.Id == criteria.SettingId)
                .ToList()
                .FirstOrDefault();
        }
    }
}
