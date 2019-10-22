namespace Fanex.CRM.AdminTool.Repositories
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.CommunicationToolSetting;
    using Fanex.CRM.AdminTool.Models.CommunicationToolSetting;
    using Fanex.Data;

    public interface ICommunicationToolSettingRepository
    {
        IEnumerable<CommunicationToolSettingItem> GetCommunicationToolSetting(GetCommunicationToolSettingCriteria criteria);
    }

    public class CommunicationToolSettingRepository : ICommunicationToolSettingRepository
    {
        public IEnumerable<CommunicationToolSettingItem> GetCommunicationToolSetting(GetCommunicationToolSettingCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                var settingItems = objectDB.Query<CommunicationToolSettingItem>(criteria.GetSpParams());
                Hashtable outputParams = objectDB.GetOuputParams();
                if (outputParams.Count != 0)
                {
                    criteria.TotalRows = Converter.ToInt(outputParams["totalrows"]);
                }

                return settingItems;
            }
        }
    }
}