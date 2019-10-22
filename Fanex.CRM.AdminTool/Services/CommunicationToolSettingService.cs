namespace Fanex.CRM.AdminTool.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Models.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Repositories;

    public interface ICommunicationToolSettingService
    {
        Tuple<IEnumerable<CommunicationToolSettingItem>, int> GetCommunicationToolSetting();
    }

    public class CommunicationToolSettingService : ICommunicationToolSettingService
    {
        private readonly ICommunicationToolSettingRepository repository;

        public CommunicationToolSettingService() : this(null)
        {
        }

        public CommunicationToolSettingService(ICommunicationToolSettingRepository repository)
        {
            this.repository = repository ?? new CommunicationToolSettingRepository();
        }

        public Tuple<IEnumerable<CommunicationToolSettingItem>, int> GetCommunicationToolSetting()
        {
            var criteria = new GetCommunicationToolSettingCriteria
            {
            };
            IEnumerable<CommunicationToolSettingItem> settingItems = repository.GetCommunicationToolSetting(criteria);
            return new Tuple<IEnumerable<CommunicationToolSettingItem>, int>(settingItems, criteria.TotalRows);
        }
    }
}