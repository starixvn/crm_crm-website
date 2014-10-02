// --------------------------------------------------------------------------
// <copyright file="SettingRelationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingRelation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.AdminTool.Repositories.SettingRelation;
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
    public class SettingRelationService : ISettingRelationService
    {
        private const string AllSettingSessionKey = "Fanex.CRM.AdminTool.Services.SettingRelation.GetAllSettingSessionKey";

        private const string AllSettingRelationSessionKey = "Fanex.CRM.AdminTool.Services.SettingRelation.GetAllSettingRelationSessionKey";

        private ISettingRelationRepository _settingRelationRepository;

        private ISettingService _settingService;

        public SettingRelationService()
            : this(null, null)
        {
        }

        public SettingRelationService(
            ISettingRelationRepository settingRelationRepository,
            ISettingService settingService)
        {
            _settingRelationRepository = settingRelationRepository ?? new SettingRelationRepository();
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

                result.ForEach(s => s.ChildSettings = GetSettingByParentId(s.Id));
                return result.OrderBy(s => s.DisplayOrder).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the main setting for add relation.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main setting for add relation.</returns>
        public List<SettingInfo> GetMainSettingForAddRelation(GetMainSettingForAddRelationCriteria criteria)
        {
            try
            {
                var mainSetting = GetMainSetting();
                return mainSetting
                    .Where(s => !string.Equals(s.Type, criteria.MainSetting.Type, StringComparison.Ordinal))
                    .Select(s => s)
                    .ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the main setting relations.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The main setting relations.</returns>
        public List<SettingInfo> MainSettingRelations(GetMainSettingRelationsCriteria criteria)
        {
            try
            {
                return _settingRelationRepository.MainSettingRelations(criteria);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        /// <summary>
        /// Adds the setting relations.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Add result.</returns>
        public Result AddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            try
            {
                var addResult = _settingRelationRepository.AddSettingRelations(criteria);
                if (addResult.Code == ErrorCodeConstants.NoneError)
                {
                    addResult.Message = Labels.SettingRelation_AddRelation_UpdateSuccess;
                    CacheService.Delete(AllSettingSessionKey);
                    CacheService.Delete(AllSettingRelationSessionKey);
                    _settingService.ResetAllCache();
                }

                return addResult;
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
                allSetting = _settingRelationRepository.AllSetting(new GetAllSettingCriteria());
                CacheService.Set(AllSettingSessionKey, allSetting);
            }

            return allSetting;
        }

        private List<SettingInfo> AllSettingRelation()
        {
            var allSettingRelation = CacheService.Get<List<SettingInfo>>(AllSettingRelationSessionKey);
            if (allSettingRelation == null)
            {
                allSettingRelation = _settingRelationRepository.AllSettingRelation(new GetAllSettingRelationCriteria());
                CacheService.Set(AllSettingRelationSessionKey, allSettingRelation);
            }

            return allSettingRelation;
        }

        private void GetSubSetting(string key, List<SettingInfo> settingList, IEnumerable<SettingInfo> allSetting)
        {
            var subList = allSetting.Where(s => s.Type == key).Select(s => s).OrderBy(s => s.DisplayOrder).ToList();
            var setting = new SettingInfo { Type = key, ChildSettings = subList };
            settingList.Add(setting);
        }

        /// <summary>
        /// Gets the setting by parent identifier.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>The setting by parent identifier.</returns>
        private List<SettingInfo> GetSettingByParentId(int parentId)
        {
            var allSettingRelation = AllSettingRelation();
            var allSettings = AllSetting();

            var settingIds = allSettingRelation
                .Where(r => r.ParentId == parentId)
                .Select(s => new { s.Id, s.DisplayOrder });

            var query = allSettings.AsQueryable().Join(
                settingIds,
                setting => setting.Id,
                parent => parent.Id,
                (setting, parent) =>
                    new SettingInfo
                    {
                        Id = setting.Id,
                        DisplayOrder = parent.DisplayOrder,
                        Value = setting.Value,
                        Type = setting.Type
                    })
                    .ToList()
                    .OrderBy(s => s.DisplayOrder);

            return query.ToList();
        }
    }
}
