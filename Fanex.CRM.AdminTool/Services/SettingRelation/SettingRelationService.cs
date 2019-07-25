// --------------------------------------------------------------------------
// <copyright file="SettingRelationService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Services.SettingRelation
{
    using Fanex.CRM.AdminTool.Criteria.SettingRelation;
    using Fanex.CRM.AdminTool.Repositories.SettingRelation;
    using Fanex.CRM.AdminTool.Resources;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;
    using Fanex.CRM.Infrastructure.Constants;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Services;
    using Models.SettingManagement;
    using Models.SettingRelation;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public IEnumerable<Setting> GetMainSettings()
        {
            return _settingService.GetMainSettings();
        }

        public IEnumerable<Setting> GetSettingByTypes(GetSettingByTypeCriteria criteria)
        {
            var allSettings = _settingService.GetAllSettings();
            var result = allSettings
                .Where(s => s.SettingType == criteria.MainSetting.SettingType)
                .OrderBy(s => s.DisplayOrder)
                .ToList();

            result.ForEach(s => s.ChildSettings = GetSettingByParentId(s.Id).ToList());
            return result;
        }

        public IEnumerable<Setting> GetMainSettingForAddRelations(GetMainSettingForAddRelationCriteria criteria)
        {
            var mainSetting = GetMainSettings();
            return mainSetting
                .Where(s => !string.Equals(s.SettingType, criteria.MainSetting.SettingType, StringComparison.Ordinal))
                .ToList();
        }

        public IEnumerable<SettingRelation> MainSettingRelations(GetMainSettingRelationsCriteria criteria)
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

        public IEnumerable<SettingRelationItem> MainSettingRelationsCN88(GetMainSettingRelationsCriteria criteria)
        {
            var parentSetting = _settingRelationRepository.GetSettingByTypeCN88(new GetSettingByTypeCN88Criteria
            {
                SettingName = criteria.ParentName
            });

            var settingByTypeData = _settingRelationRepository.GetSettingByTypeCN88(new GetSettingByTypeCN88Criteria
            {
                SettingName = criteria.MainSetting.SettingType
            });

            var result = settingByTypeData
                .Where(s => s.ParentID == 0)
                .Select(s =>
                {
                    var settingRelationItem = new SettingRelationItem
                    {
                        SettingID = s.SettingID,
                        SettingValue = s.SettingValue,
                        SettingValueCS = s.SettingValueCS,
                        SettingValueCT = s.SettingValueCT,
                        DisplayOrder = s.DisplayOrder,
                        IsSub = parentSetting.Any(p => p.SettingID == s.SettingID && p.ParentID == criteria.ParentId)
                    };

                    return settingRelationItem;
                })
                .ToList();

            return result;
        }

        public List<MainSetting> GetMainSettingCN88(GetMainSettingCriteria criteria)
        {
            return _settingRelationRepository.GetMainSettingCN88(criteria);
        }

        public List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria)
        {
            var data = _settingRelationRepository.GetSettingByTypeCN88(criteria);
            var result = data
                .Where(s => s.ParentID == 0)
                .Select(s =>
                {
                    s.ChildSettings = data.Where(c => c.ParentID == s.SettingID).ToList();
                    return s;
                })
                .ToList();

            return result;
        }

        private IEnumerable<Setting> GetAllSettingRelations()
        {
            var settingRelations = CacheService.Get<IEnumerable<Setting>>(AllSettingRelationSessionKey);
            if (settingRelations == null)
            {
                settingRelations = _settingRelationRepository.GetAllSettingRelations(new GetAllSettingRelationCriteria());
                CacheService.Set(AllSettingRelationSessionKey, settingRelations);
            }

            return settingRelations;
        }

        private IEnumerable<Setting> GetSettingByParentId(int parentId)
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
                    new Setting
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
