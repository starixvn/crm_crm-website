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
        Tuple<IEnumerable<CommunicationToolSettingItem>, int> GetAll(GetCommunicationToolSettingCriteria criteria);

        CommunicationToolSettingItem GetById(int id);

        CommandResult Create(CreateCommunicationToolSettingCriteria criteria);

        CommandResult Edit(EditCommunicationToolSettingCriteria criteria);

        CommandResult Delete(int id);
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

        public Tuple<IEnumerable<CommunicationToolSettingItem>, int> GetAll(GetCommunicationToolSettingCriteria criteria)
        {
            IEnumerable<CommunicationToolSettingItem> settingItems = repository.GetAll(criteria);
            return new Tuple<IEnumerable<CommunicationToolSettingItem>, int>(settingItems, criteria.TotalRows);
        }

        public CommunicationToolSettingItem GetById(int id)
        {
            var criteria = new GetCommunicationToolSettingByIdCriteria
            {
                DefaultCommId = id
            };
            return baseService.GetDataEntity<CommunicationToolSettingItem>(criteria);
        }

        public CommandResult Create(CreateCommunicationToolSettingCriteria criteria)
        {
            return baseService.ExecuteNonQuery(criteria);
        }

        public CommandResult Edit(EditCommunicationToolSettingCriteria criteria)
        {
            return baseService.ExecuteNonQuery(criteria);
        }

        public CommandResult Delete(int id)
        {
            var criteria = new DeleteCommunicationToolSettingCriteria
            {
                DefaultCommId = id
            };
            return baseService.ExecuteNonQuery(criteria);
        }
    }
}