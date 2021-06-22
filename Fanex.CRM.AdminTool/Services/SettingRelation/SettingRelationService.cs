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
    using Fanex.CRM.Setting.Services;
    using Models.SettingManagement;
    using Models.SettingRelation;

    public class SettingRelationService : ISettingRelationService
    {
        private const string AllSettingRelationSessionKey = "Fanex.CRM.AdminTool.Services.SettingRelation.GetAllSettingRelationSessionKey";

        private readonly ISettingRelationRepository _settingRelationRepository;
        private readonly ISettingService _settingService;
        private readonly IBaseService baseService;

        public SettingRelationService() : this(null, null, null)
        {
        }

        public SettingRelationService(
            ISettingRelationRepository settingRelationRepository,
            IBaseService baseService,
            ISettingService settingService)
        {
            _settingRelationRepository = settingRelationRepository ?? new SettingRelationRepository();
            _settingService = settingService ?? new SettingService();
            this.baseService = baseService ?? new BaseService();
        }

        public IEnumerable<Setting> GetMainSettings()
        {
            return _settingService.GetMainSettings();
        }

        public IEnumerable<Setting> GetSettingByTypes(GetSettingByTypeCriteria criteria)
        {
            IEnumerable<Setting> allSettings = _settingService.GetAllSettings();
            IEnumerable<Setting> relations = GetAllSettingRelations();
            IEnumerable<Setting> result = allSettings
                .Where(s => s.SettingType == criteria.MainSetting.SettingType)
                .GroupBy(s => s.Id).Select(x => x.First())
                .OrderBy(s => s.DisplayOrder);

            foreach(Setting item in result)
            {
                var settingIds = relations
                    .Where(r => r.ParentId == item.Id)
                    .Select(s => new { s.Id, s.DisplayOrder });
                if (settingIds.Any())
                {
                    item.ChildSettings = allSettings.Where(x => settingIds.Any(y => x.Id == y.Id))
                        .GroupBy(x => x.Id)
                        .Select(x => x.First())
                        .OrderBy(s => s.DisplayOrder).ToList();
                }
            }

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

        public CommandResult GetAddSettingRelations(AddSettingRelationsCriteria criteria)
        {
            CommandResult result = baseService.ExecuteNonQuery(criteria);
            if (result.IsSuccessful)
            {
                result.Message = Labels.SettingRelation_AddRelation_UpdateSuccess;
                _settingService.ClearAllCaches(AllSettingRelationSessionKey);
            }

            return result;
        }

        public IEnumerable<SettingRelationItem> MainSettingRelationsCN88(GetMainSettingRelationsCriteria criteria)
        {
            var parentSetting = baseService.GetData<SettingItemCN88>(new GetSettingByTypeCN88Criteria
            {
                SettingName = criteria.ParentName
            });

            var settingByTypeData = baseService.GetData<SettingItemCN88>(new GetSettingByTypeCN88Criteria
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
            return baseService.GetData<MainSetting>(criteria).ToList();
        }

        public List<SettingItemCN88> GetSettingByTypeCN88(GetSettingByTypeCN88Criteria criteria)
        {
            var data = baseService.GetData<SettingItemCN88>(criteria);
            var result = data
                .Where(s => s.ParentID == 0)
                .Select(s =>
                {
                    s.ChildSettings = data.Where(c => c.ParentID == s.SettingID).ToList();
                    return s;
                }).ToList();

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
    }
}