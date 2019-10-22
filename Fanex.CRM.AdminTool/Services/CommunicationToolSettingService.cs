namespace Fanex.CRM.AdminTool.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Models.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Repositories;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Common.Services;

    public interface ICommunicationToolSettingService
    {
        Tuple<IEnumerable<CommunicationToolSettingItem>, int> GetCommunicationToolSetting(GetCommunicationToolSettingCriteria criteria);

        CommandResult Create(CreateCommunicationToolSettingCriteria criteria);
    }

    public class CommunicationToolSettingService : ICommunicationToolSettingService
    {
        private readonly ICommunicationToolSettingRepository repository;
        private readonly IBaseService baseService;

        public CommunicationToolSettingService() : this(null, null)
        {
        }

        public CommunicationToolSettingService(
            ICommunicationToolSettingRepository repository,
            IBaseService baseService)
        {
            this.repository = repository ?? new CommunicationToolSettingRepository();
            this.baseService = baseService ?? new BaseService();
        }

        public Tuple<IEnumerable<CommunicationToolSettingItem>, int> GetCommunicationToolSetting(GetCommunicationToolSettingCriteria criteria)
        {
            IEnumerable<CommunicationToolSettingItem> settingItems = repository.GetCommunicationToolSetting(criteria);
            return new Tuple<IEnumerable<CommunicationToolSettingItem>, int>(settingItems, criteria.TotalRows);
        }

        public CommandResult Create(CreateCommunicationToolSettingCriteria criteria)
        {
            return baseService.ExecuteNonQuery(criteria);
        }
    }
}