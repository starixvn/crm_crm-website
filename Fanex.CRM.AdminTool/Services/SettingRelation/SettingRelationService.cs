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
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services;

    /// <summary>
    /// Setting Relation Service.
    /// </summary>
    public class SettingRelationService : ISettingRelationService
    {
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

        public IEnumerable<SettingInfo> GetMainSettings()
        {
            return _settingService.GetMainSettings();
        }

        public IEnumerable<SettingInfo> GetSettingByTypes(GetSettingByTypeCriteria criteria)
        {
            var allSettings = _settingService.GetAllSettings();
            var result = allSettings
                .Where(s => s.SettingType == criteria.MainSetting.SettingType)
                .OrderBy(s => s.DisplayOrder)
                .ToList();

            result.ForEach(s => s.ChildSettings = GetSettingByParentId(s.Id).ToList());
            return result;
        }

        public IEnumerable<SettingInfo> GetMainSettingForAddRelations(GetMainSettingForAddRelationCriteria criteria)
        {
            var mainSetting = GetMainSettings();
            return mainSetting
                .Where(s => !string.Equals(s.SettingType, criteria.MainSetting.SettingType, StringComparison.Ordinal))
                .ToList();
        }

        public IEnumerable<SettingInfo> MainSettingRelations(GetMainSettingRelationsCriteria criteria)
        {
            return _settingRelationRepository.GetMainSettingRelations(criteria);
        }

        public Result GetAddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            var result = _settingRelationRepository.AddSettingRelations(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.SettingRelation_AddRelation_UpdateSuccess;
                CacheService.Delete(AllSettingRelationSessionKey);
                _settingService.ClearCache();
            }

            return result;
        }

        private IEnumerable<SettingInfo> GetAllSettingRelations()
        {
            var settingRelations = CacheService.Get<IEnumerable<SettingInfo>>(AllSettingRelationSessionKey);
            if (settingRelations == null)
            {
                settingRelations = _settingRelationRepository.GetAllSettingRelations(new GetAllSettingRelationCriteria());
                CacheService.Set(AllSettingRelationSessionKey, settingRelations);
            }

            return settingRelations;
        }

        private IEnumerable<SettingInfo> GetSettingByParentId(int parentId)
        {
            var allSettingRelations = GetAllSettingRelations();
            var allSettings = _settingService.GetAllSettings();

            var settingIds = allSettingRelations
                .Where(r => r.ParentId == parentId)
                .Select(s => new { s.Id, s.DisplayOrder });

            var query = allSettings.Join(
                settingIds,
                setting => setting.Id,
                parent => parent.Id,
                (setting, parent) =>
                    new SettingInfo
                    {
                        Id = setting.Id,
                        DisplayOrder = parent.DisplayOrder,
                        Value = setting.Value,
                        SettingType = setting.SettingType
                    })
                    .OrderBy(s => s.DisplayOrder);

            return query.ToList();
        }
    }
}
